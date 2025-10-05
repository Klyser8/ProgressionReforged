using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ProgressionReforged.Content.Projectiles;
using ProgressionReforged.Systems.Mediumcore;
using Terraria.ID;
using SoulboundCache = ProgressionReforged.Content.Projectiles.SoulboundCache;

namespace ProgressionReforged.Systems.MediumcoreDeath;

internal class MediumcoreDropSystem : ModSystem
{
    internal static MediumcoreDropSystem? Instance;

    private readonly List<TagCompound> _storedDrops = new();
    private bool _pendingSpawn;

    public override void OnWorldLoad()
    {
        Instance = this;
        _storedDrops.Clear();
        _pendingSpawn = true;
    }

    public override void OnWorldUnload()
    {
        _storedDrops.Clear();
        Instance = null;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        if (_storedDrops.Count > 0)
            tag["drops"] = _storedDrops;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        _storedDrops.Clear();
        if (tag.ContainsKey("drops"))
        {
            var list = tag.GetList<TagCompound>("drops");
            foreach (var t in list)
                _storedDrops.Add(t);
        }
    }

    public override void PostUpdateWorld()
    {
        if (_pendingSpawn)
        {
            foreach (var drop in _storedDrops)
                SpawnContainerFromTag(drop);

            _pendingSpawn = false;
        }
    }

    public override void PreSaveAndQuit()
    {
        foreach (Player player in Main.player)
        {
            if (player.active && player.TryGetModPlayer(out MediumcoreDropPlayer mp) && mp.HasStoredData)
            {
                SpawnDeathContainer(player, mp.StoredData!);
                mp.StoredData = null;
            }
        }
    }

    internal void SpawnDeathContainer(Player player, TagCompound data)
    {
        if (Main.dedServ)
            return;

        TagCompound tag = new()
        {
            ["id"] = Guid.NewGuid().ToString(),
            ["origin"] = player.Center,
            ["pos"] = player.Center,
            ["data"] = data,
            ["owner"] = player.name,
            ["value"] = data.GetInt("value"),
            ["arrived"] = false
        };
        _storedDrops.Add(tag);
        SpawnContainerFromTag(tag);
    }

    internal static void SpawnContainerFromTag(TagCompound tag)
    {
        EnsureDropId(tag);

        Vector2 storedPosition = tag.ContainsKey("pos") ? tag.Get<Vector2>("pos") : Vector2.Zero;
        Vector2 searchOrigin = tag.ContainsKey("origin") ? tag.Get<Vector2>("origin") : storedPosition;
        if (!tag.ContainsKey("origin"))
            tag["origin"] = searchOrigin;

        Vector2 target;
        if (tag.ContainsKey("target"))
        {
            target = tag.Get<Vector2>("target");
        }
        else
        {
            int startX = (int)searchOrigin.X / 16;
            int startY = (int)searchOrigin.Y / 16 + 1; // bottom tile coordinate

            Point tilePos = FindSafeSpot(startX, startY);
            int x = tilePos.X;
            int y = tilePos.Y;

            if (!WorldGen.InWorld(x, y, 1))
                return;

            target = new Vector2((x + 0.5f) * 16f, (y - 2) * 16f - 22f);
            tag["target"] = target;
        }

        bool arrived = tag.ContainsKey("arrived") && tag.GetBool("arrived");
        Vector2 spawnPos = arrived ? target : storedPosition;

        tag["pos"] = spawnPos;
        tag["arrived"] = arrived;

        Vector2 velocity = Vector2.Zero;
        if (!arrived)
        {
            Vector2 offset = target - spawnPos;
            float distanceSquared = offset.LengthSquared();
            if (distanceSquared > 0f)
            {
                float distance = (float)Math.Sqrt(distanceSquared);
                velocity = offset / distance * 2f;
            }
        }

        int projIndex = Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), spawnPos, velocity, ModContent.ProjectileType<SoulboundCache>(), 0, 0f);
        if (projIndex < 0 || projIndex >= Main.maxProjectiles)
            return;

        if (Main.projectile[projIndex].ModProjectile is SoulboundCache proj)
        {
            proj.StoredData = tag.GetCompound("data");
            proj.Owner = tag.GetString("owner");
            proj.Value = tag.GetInt("value");
            proj.TargetPosition = target;
            proj.DropId = tag.GetString("id");

            if (arrived)
            {
                proj.Projectile.Center = target;
                proj.Projectile.velocity = Vector2.Zero;
                proj.Projectile.ai[1] = 1f;
            }
            else
            {
                proj.Projectile.ai[1] = 0f;
            }
        }
    }
    
    private static Point FindSafeSpot(int startX, int startY)
    {
        const int range = 20;
        for (int r = 0; r <= range; r++)
        {
            for (int dx = -r; dx <= r; dx++)
            {
                for (int dy = -r; dy <= r; dy++)
                {
                    int x = startX + dx;
                    int y = startY + dy;
                    if (!WorldGen.InWorld(x, y, 1))
                        continue;

                    if (IsSpotSafe(x, y))
                        return new Point(x, y);
                }
            }
        }

        return new Point(startX, startY);
    }

    private static bool IsSpotSafe(int x, int bottomY)
    {
        // check three tiles above the bottom coordinate
        for (int k = 0; k < 3; k++)
        {
            var t = Framing.GetTileSafely(x, bottomY - k);
            if (t.HasTile || t.LiquidType == LiquidID.Lava)
                return false;
        }

        // ensure the tile below is solid
        if (!WorldGen.SolidTile(x, bottomY + 1))
            return false;

        return true;
    }

    internal void UpdateDrop(string dropId, Vector2 position, bool arrived)
    {
        if (string.IsNullOrEmpty(dropId))
            return;

        for (int i = 0; i < _storedDrops.Count; i++)
        {
            if (_storedDrops[i].GetString("id") == dropId)
            {
                _storedDrops[i]["pos"] = position;
                _storedDrops[i]["arrived"] = arrived;
                return;
            }
        }
    }

    internal void RemoveDrop(string dropId)
    {
        if (string.IsNullOrEmpty(dropId))
            return;

        for (int i = 0; i < _storedDrops.Count; i++)
        {
            if (_storedDrops[i].GetString("id") == dropId)
            {
                _storedDrops.RemoveAt(i);
                break;
            }
        }
    }

    private static string EnsureDropId(TagCompound tag)
    {
        if (tag.ContainsKey("id"))
            return tag.GetString("id");

        string id = Guid.NewGuid().ToString();
        tag["id"] = id;
        return id;
    }
}