using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ProgressionReforged.Content.Items;
using ProgressionReforged.Content.Projectiles;
using ProgressionReforged.Content.Tiles;
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
                SpawnContainerFromTag(drop, true);

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
            ["pos"] = player.Center,
            ["data"] = data,           
            ["owner"] = player.name,
            ["value"] = data.GetInt("value")
        };
        _storedDrops.Add(tag);
        SpawnContainerFromTag(tag);
    }

    internal static void SpawnContainerFromTag(TagCompound tag, bool fromWorldLoad = false)
    {
        Vector2 origin = tag.Get<Vector2>("pos");
        int startX = (int)origin.X / 16;
        int startY = (int)origin.Y / 16 + 1; // bottom tile coordinate

        Point tilePos = FindSafeSpot(startX, startY);
        int x = tilePos.X;
        int y = tilePos.Y;

        if (!WorldGen.InWorld(x, y, 1))
            return;

        // position the projectile so its bottom rests just above the tile top
        Vector2 finalPos = new Vector2((x + 0.5f) * 16f, (y - 2) * 16f - 22f);
        tag["pos"] = finalPos;
        tag["tileX"] = x;
        tag["tileY"] = y;

        Vector2 spawnPos = fromWorldLoad ? finalPos : origin;
        Vector2 velocity = Vector2.Normalize(finalPos - origin);
        if (!velocity.HasNaNs())
            velocity *= fromWorldLoad ? 0f : 2f;
        else
            velocity = Vector2.Zero;
        
        if (fromWorldLoad && !Framing.GetTileSafely(x, y).HasTile)
            WorldGen.PlaceTile(x, y, ModContent.TileType<Content.Tiles.SoulboundCache>(), false, true);

        int id = Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), spawnPos, velocity, ModContent.ProjectileType<SoulboundCache>(), 0, 0f);        
        if (id < Main.maxProjectiles && id >= 0)
        {
            if (Main.projectile[id].ModProjectile is SoulboundCache proj)
            {
                proj.StoredData = tag.GetCompound("data");
                proj.Owner = tag.GetString("owner");
                proj.Value = tag.GetInt("value");
                proj.tileX = x;
                proj.tileY = y;
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

    internal void RemoveDropAt(int tileX, int tileY)
    {
        for (int i = 0; i < _storedDrops.Count; i++)
        {
            int x = _storedDrops[i].GetInt("tileX");
            int y = _storedDrops[i].GetInt("tileY");
            if (x == tileX && y == tileY)
            {
                _storedDrops.RemoveAt(i);
                break;
            }
        }
    }
}