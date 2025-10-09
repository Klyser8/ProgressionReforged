using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace ProgressionReforged.Systems.LifeCrystals;

internal sealed class LifeCrystalSystem : ModSystem
{
    private bool _kingSlimeRewardUnlocked;
    private int _kingSlimePendingCrystals;

    private bool _eyeOfCthulhuRewardUnlocked;
    private int _eyeOfCthulhuPendingCrystals;

    private bool _eaterOfWorldsRewardUnlocked;
    private int _eaterOfWorldsPendingCrystals;

    private bool _brainOfCthulhuRewardUnlocked;
    private int _brainOfCthulhuPendingCrystals;

    private bool _skeletronRewardUnlocked;
    private int _skeletronPendingCrystals;

    public override void OnWorldLoad()
    {
        _kingSlimeRewardUnlocked = false;
        _kingSlimePendingCrystals = 0;

        _eyeOfCthulhuRewardUnlocked = false;
        _eyeOfCthulhuPendingCrystals = 0;

        _eaterOfWorldsRewardUnlocked = false;
        _eaterOfWorldsPendingCrystals = 0;

        _brainOfCthulhuRewardUnlocked = false;
        _brainOfCthulhuPendingCrystals = 0;

        _skeletronRewardUnlocked = false;
        _skeletronPendingCrystals = 0;
    }

    public override void OnWorldUnload()
    {
        OnWorldLoad();
    }

    public override void LoadWorldData(TagCompound tag)
    {
        if (tag.ContainsKey(nameof(_kingSlimeRewardUnlocked)))
        {
            _kingSlimeRewardUnlocked = tag.GetBool(nameof(_kingSlimeRewardUnlocked));
        }

        if (tag.ContainsKey(nameof(_kingSlimePendingCrystals)))
        {
            _kingSlimePendingCrystals = Math.Max(0, tag.GetInt(nameof(_kingSlimePendingCrystals)));
        }

        if (tag.ContainsKey(nameof(_eyeOfCthulhuRewardUnlocked)))
        {
            _eyeOfCthulhuRewardUnlocked = tag.GetBool(nameof(_eyeOfCthulhuRewardUnlocked));
        }

        if (tag.ContainsKey(nameof(_eyeOfCthulhuPendingCrystals)))
        {
            _eyeOfCthulhuPendingCrystals = Math.Max(0, tag.GetInt(nameof(_eyeOfCthulhuPendingCrystals)));
        }

        if (tag.ContainsKey(nameof(_eaterOfWorldsRewardUnlocked)))
        {
            _eaterOfWorldsRewardUnlocked = tag.GetBool(nameof(_eaterOfWorldsRewardUnlocked));
        }

        if (tag.ContainsKey(nameof(_eaterOfWorldsPendingCrystals)))
        {
            _eaterOfWorldsPendingCrystals = Math.Max(0, tag.GetInt(nameof(_eaterOfWorldsPendingCrystals)));
        }

        if (tag.ContainsKey(nameof(_brainOfCthulhuRewardUnlocked)))
        {
            _brainOfCthulhuRewardUnlocked = tag.GetBool(nameof(_brainOfCthulhuRewardUnlocked));
        }

        if (tag.ContainsKey(nameof(_brainOfCthulhuPendingCrystals)))
        {
            _brainOfCthulhuPendingCrystals = Math.Max(0, tag.GetInt(nameof(_brainOfCthulhuPendingCrystals)));
        }

        if (tag.ContainsKey(nameof(_skeletronRewardUnlocked)))
        {
            _skeletronRewardUnlocked = tag.GetBool(nameof(_skeletronRewardUnlocked));
        }

        if (tag.ContainsKey(nameof(_skeletronPendingCrystals)))
        {
            _skeletronPendingCrystals = Math.Max(0, tag.GetInt(nameof(_skeletronPendingCrystals)));
        }
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag[nameof(_kingSlimeRewardUnlocked)] = _kingSlimeRewardUnlocked;
        tag[nameof(_kingSlimePendingCrystals)] = _kingSlimePendingCrystals;

        tag[nameof(_eyeOfCthulhuRewardUnlocked)] = _eyeOfCthulhuRewardUnlocked;
        tag[nameof(_eyeOfCthulhuPendingCrystals)] = _eyeOfCthulhuPendingCrystals;

        tag[nameof(_eaterOfWorldsRewardUnlocked)] = _eaterOfWorldsRewardUnlocked;
        tag[nameof(_eaterOfWorldsPendingCrystals)] = _eaterOfWorldsPendingCrystals;

        tag[nameof(_brainOfCthulhuRewardUnlocked)] = _brainOfCthulhuRewardUnlocked;
        tag[nameof(_brainOfCthulhuPendingCrystals)] = _brainOfCthulhuPendingCrystals;

        tag[nameof(_skeletronRewardUnlocked)] = _skeletronRewardUnlocked;
        tag[nameof(_skeletronPendingCrystals)] = _skeletronPendingCrystals;
    }

    public override void PostWorldGen()
    {
        ReduceInitialLifeCrystals();
    }

    public override void PostUpdateWorld()
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            return;
        }

        var config = ProgressionReforgedConfig.Instance;

        ProcessBossReward(ref _kingSlimeRewardUnlocked, ref _kingSlimePendingCrystals, NPC.downedSlimeKing, config.KingSlimeLifeCrystals);
        ProcessBossReward(ref _eyeOfCthulhuRewardUnlocked, ref _eyeOfCthulhuPendingCrystals, NPC.downedBoss1, config.EyeOfCthulhuLifeCrystals);
        ProcessBossReward(ref _eaterOfWorldsRewardUnlocked, ref _eaterOfWorldsPendingCrystals, NPC.downedBoss2 && !WorldGen.crimson, config.EaterOfWorldsLifeCrystals);
        ProcessBossReward(ref _brainOfCthulhuRewardUnlocked, ref _brainOfCthulhuPendingCrystals, NPC.downedBoss2 && WorldGen.crimson, config.BrainOfCthulhuLifeCrystals);
        ProcessBossReward(ref _skeletronRewardUnlocked, ref _skeletronPendingCrystals, NPC.downedBoss3, config.SkeletronLifeCrystals);
    }

    public static int CountLifeCrystals()
    {
        int count = 0;

        for (int x = 0; x < Main.maxTilesX; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Tile tile = Main.tile[x, y];
                if (tile.HasTile && tile.TileType == TileID.Heart && tile.TileFrameX == 0 && tile.TileFrameY == 0)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static void ReduceInitialLifeCrystals()
    {
        var config = ProgressionReforgedConfig.Instance;
        float multiplier = Math.Clamp(config.InitialLifeCrystalMultiplier, 0f, 1f);

        if (multiplier >= 0.999f)
        {
            return;
        }

        List<Point16> heartLocations = new();
        for (int x = 0; x < Main.maxTilesX; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Tile tile = Main.tile[x, y];
                if (tile.HasTile && tile.TileType == TileID.Heart && tile.TileFrameX == 0 && tile.TileFrameY == 0)
                {
                    heartLocations.Add(new Point16(x, y));
                }
            }
        }

        if (heartLocations.Count == 0)
        {
            return;
        }

        UnifiedRandom random = WorldGen.genRand ?? Main.rand;
        int heartsToKeep = (int)Math.Ceiling(heartLocations.Count * multiplier);
        int heartsToRemove = Math.Max(0, heartLocations.Count - heartsToKeep);

        for (int i = 0; i < heartsToRemove; i++)
        {
            int index = random.Next(heartLocations.Count);
            Point16 topLeft = heartLocations[index];
            heartLocations.RemoveAt(index);

            RemoveHeartAt(topLeft);
        }
    }

    private static void RemoveHeartAt(Point16 topLeft)
    {
        for (int xOffset = 0; xOffset < 2; xOffset++)
        {
            for (int yOffset = 0; yOffset < 2; yOffset++)
            {
                int x = topLeft.X + xOffset;
                int y = topLeft.Y + yOffset;
                if (WorldGen.InWorld(x, y))
                {
                    WorldGen.KillTile(x, y, noItem: true);
                }
            }
        }

        NetMessage.SendTileSquare(-1, topLeft.X + 1, topLeft.Y + 1, 2);
    }

    private void ProcessBossReward(ref bool unlocked, ref int pendingCrystals, bool conditionMet, int configuredAmount)
    {
        if (conditionMet && !unlocked)
        {
            unlocked = true;
            pendingCrystals += Math.Max(0, configuredAmount);
        }

        if (pendingCrystals <= 0)
        {
            return;
        }

        int attemptCount = Math.Min(pendingCrystals, 5);
        int placed = SpawnLifeCrystals(attemptCount);

        if (placed > 0)
        {
            pendingCrystals -= placed;
            AnnounceLifeCrystalsSpawned(placed);
        }
    }

    private int SpawnLifeCrystals(int amount)
    {
        int placed = 0;
        const int attemptsPerCrystal = 3000;
        for (int i = 0; i < amount; i++)
        {
            if (TryPlaceLifeCrystal(attemptsPerCrystal))
            {
                placed++;
            }
        }

        return placed;
    }

    private bool TryPlaceLifeCrystal(int attempts)
    {
        UnifiedRandom random = WorldGen.genRand ?? Main.rand;

        for (int attempt = 0; attempt < attempts; attempt++)
        {
            int x = random.Next(50, Main.maxTilesX - 50);
            int y = random.Next((int)Main.rockLayer, Main.maxTilesY - 200);

            if (!WorldGen.InWorld(x, y, 2))
            {
                continue;
            }

            if (!IsSuitableLifeCrystalLocation(x, y))
            {
                continue;
            }

            if (WorldGen.PlaceTile(x, y, TileID.Heart, mute: true, forced: true))
            {
                WorldGen.SquareTileFrame(x, y, resetFrame: true);
                WorldGen.SquareTileFrame(x + 1, y, resetFrame: true);
                WorldGen.SquareTileFrame(x, y + 1, resetFrame: true);
                WorldGen.SquareTileFrame(x + 1, y + 1, resetFrame: true);

                NetMessage.SendTileSquare(-1, x + 1, y + 1, 2);
                return true;
            }
        }

        return false;
    }

    private static bool IsSuitableLifeCrystalLocation(int x, int y)
    {
        if (y < Main.worldSurface)
        {
            return false;
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Tile tile = Framing.GetTileSafely(x + i, y + j);
                if (tile.HasTile || tile.LiquidAmount > 0 || tile.IsActuated)
                {
                    return false;
                }
            }
        }

        Tile belowLeft = Framing.GetTileSafely(x, y + 2);
        Tile belowRight = Framing.GetTileSafely(x + 1, y + 2);

        return IsSupportingTile(belowLeft) && IsSupportingTile(belowRight);
    }

    private static bool IsSupportingTile(Tile tile)
    {
        if (!tile.HasTile || tile.IsActuated)
        {
            return false;
        }

        if (!Main.tileSolid[tile.TileType])
        {
            return false;
        }

        return tile.Slope == SlopeType.Solid;
    }

    private static void AnnounceLifeCrystalsSpawned(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        string message = Language.GetTextValue("Mods.ProgressionReforged.LifeCrystals.BossReward", amount);
        Color color = new(255, 240, 20);

        if (Main.netMode == NetmodeID.Server)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
        }
        else
        {
            Main.NewText(message, color.R, color.G, color.B);
        }
    }

    public static int GetLifeCrystalCostForNextHeart(Player player)
    {
        var config = ProgressionReforgedConfig.Instance;

        int currentHearts = Math.Max(0, (player.statLifeMax - 100) / 20);
        int nextHeartIndex = currentHearts + 1;

        int baseCost = Math.Max(1, config.LifeCrystalBaseCost);
        int frequency = Math.Max(1, config.LifeCrystalCostIncreaseFrequency);
        int increment = Math.Max(0, config.LifeCrystalCostIncreaseAmount);

        int steps = (nextHeartIndex - 1) / frequency;
        long cost = baseCost + (long)steps * increment;

        return (int)Math.Clamp(cost, 1, 999);
    }
}
