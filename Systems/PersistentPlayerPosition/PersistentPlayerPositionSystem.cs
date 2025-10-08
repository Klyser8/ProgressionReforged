using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ProgressionReforged.Systems.PersistentPlayerPosition;

internal sealed class PersistentPlayerPositionSystem : ModSystem
{
    internal static PersistentPlayerPositionSystem? Instance;

    private readonly Dictionary<string, Vector2> _storedPositions = new();
    private bool[] _hasEnteredWorld = Array.Empty<bool>();
    private bool[] _pendingPositionRestore = Array.Empty<bool>();

    public override void OnWorldLoad()
    {
        Instance = this;
        _storedPositions.Clear();
        _hasEnteredWorld = new bool[Main.maxPlayers];
        _pendingPositionRestore = new bool[Main.maxPlayers];
    }

    public override void OnWorldUnload()
    {
        _storedPositions.Clear();
        _hasEnteredWorld = Array.Empty<bool>();
        _pendingPositionRestore = Array.Empty<bool>();
        Instance = null;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        if (_storedPositions.Count == 0)
            return;

        List<TagCompound> entries = new();
        foreach ((string key, Vector2 value) in _storedPositions)
        {
            entries.Add(new TagCompound
            {
                ["id"] = key,
                ["position"] = value
            });
        }

        tag["persistentPlayerPositions"] = entries;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        _storedPositions.Clear();
        if (!tag.ContainsKey("persistentPlayerPositions"))
            return;

        foreach (TagCompound entry in tag.GetList<TagCompound>("persistentPlayerPositions"))
        {
            if (!entry.ContainsKey("id") || !entry.ContainsKey("position"))
                continue;

            string key = entry.GetString("id");
            Vector2 position = entry.Get<Vector2>("position");
            if (!string.IsNullOrWhiteSpace(key))
                _storedPositions[key] = position;
        }
    }

    public override void PostUpdatePlayers()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        EnsureCapacity();

        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];
            if (player is null || !player.active)
            {
                if ((uint)i < _hasEnteredWorld.Length)
                {
                    _hasEnteredWorld[i] = false;
                    _pendingPositionRestore[i] = false;
                }
                continue;
            }

            if (!_hasEnteredWorld[i])
            {
                _hasEnteredWorld[i] = true;
                _pendingPositionRestore[i] = true;
            }

            if (_pendingPositionRestore[i])
            {
                _pendingPositionRestore[i] = false;
                if (TryGetStoredPosition(player, out Vector2 position))
                    RestorePlayerPosition(player, position);
            }

            StorePlayerPosition(player);
        }
    }

    private void StorePlayerPosition(Player player)
    {
        string key = GetPlayerKey(player);
        if (string.IsNullOrWhiteSpace(key))
            return;

        _storedPositions[key] = player.position;
    }

    private bool TryGetStoredPosition(Player player, out Vector2 position)
    {
        string key = GetPlayerKey(player);
        if (!string.IsNullOrWhiteSpace(key) && _storedPositions.TryGetValue(key, out Vector2 stored))
        {
            position = stored;
            return true;
        }

        position = default;
        return false;
    }

    private void EnsureCapacity()
    {
        if (_hasEnteredWorld.Length == Main.maxPlayers)
            return;

        Array.Resize(ref _hasEnteredWorld, Main.maxPlayers);
        Array.Resize(ref _pendingPositionRestore, Main.maxPlayers);
    }

    private static string GetPlayerKey(Player player)
    {
        return string.IsNullOrWhiteSpace(player.name) ? string.Empty : player.name;
    }

    private static void RestorePlayerPosition(Player player, Vector2 position)
    {
        int tileX = (int)(position.X / 16f);
        int tileY = (int)(position.Y / 16f);
        if (!WorldGen.InWorld(tileX, tileY, 1))
            return;

        player.Teleport(position, TeleportationStyleID.RodOfDiscord);
        player.fallStart = (int)(player.position.Y / 16f);

        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, player.position.X, player.position.Y, 1);
    }
}