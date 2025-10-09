using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace ProgressionReforged.Systems.LifeCrystals;

internal sealed class LifeCrystalSystem : ModSystem
{
    private bool _kingSlimeRewardGiven;
    private bool _eyeOfCthulhuRewardGiven;
    private bool _evilBossRewardGiven;
    private bool _queenBeeRewardGiven;
    private bool _deerClopsRewardGiven;
    private bool _skeletronRewardGiven;

    public override void OnWorldLoad()
    {
        _kingSlimeRewardGiven = false;
        _eyeOfCthulhuRewardGiven = false;
        _evilBossRewardGiven = false;
        _queenBeeRewardGiven = false;
        _deerClopsRewardGiven = false;
        _skeletronRewardGiven = false;
    }

    public override void OnWorldUnload()
    {
        OnWorldLoad();
    }

    public override void LoadWorldData(TagCompound tag)
    {
        _kingSlimeRewardGiven = tag.GetBool(nameof(_kingSlimeRewardGiven));
        _eyeOfCthulhuRewardGiven = tag.GetBool(nameof(_eyeOfCthulhuRewardGiven));
        _evilBossRewardGiven = tag.GetBool(nameof(_evilBossRewardGiven));
        _queenBeeRewardGiven = tag.GetBool(nameof(_queenBeeRewardGiven));
        _deerClopsRewardGiven = tag.GetBool(nameof(_deerClopsRewardGiven));
        _skeletronRewardGiven = tag.GetBool(nameof(_skeletronRewardGiven));
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag[nameof(_kingSlimeRewardGiven)] = _kingSlimeRewardGiven;
        tag[nameof(_eyeOfCthulhuRewardGiven)] = _eyeOfCthulhuRewardGiven;
        tag[nameof(_evilBossRewardGiven)] = _evilBossRewardGiven;
        tag[nameof(_queenBeeRewardGiven)] = _queenBeeRewardGiven;
        tag[nameof(_deerClopsRewardGiven)] = _deerClopsRewardGiven;
        tag[nameof(_skeletronRewardGiven)] = _skeletronRewardGiven;
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

        TryGrantBossReward(ref _kingSlimeRewardGiven, NPC.downedSlimeKing, config.KingSlimeLifeCrystals);
        TryGrantBossReward(ref _eyeOfCthulhuRewardGiven, NPC.downedBoss1, config.EyeOfCthulhuLifeCrystals);
        TryGrantBossReward(ref _evilBossRewardGiven, NPC.downedBoss2, config.EvilBossLifeCrystals);
        TryGrantBossReward(ref _queenBeeRewardGiven, NPC.downedQueenBee, config.QueenBeeLifeCrystals);
        TryGrantBossReward(ref _deerClopsRewardGiven, NPC.downedDeerclops, config.DeerclopsLifeCrystals);
        TryGrantBossReward(ref _skeletronRewardGiven, NPC.downedBoss3, config.SkeletronLifeCrystals);
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

    private void TryGrantBossReward(ref bool flag, bool conditionMet, int amount)
    {
        if (flag || !conditionMet || amount <= 0)
        {
            return;
        }

        int placed = SpawnLifeCrystals(amount);
        if (placed > 0)
        {
            flag = true;
        }
    }

    private int SpawnLifeCrystals(int amount)
    {
        int placed = 0;
        var attemptsPerCrystal = 3000;
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
                if (tile.HasTile)
                {
                    return false;
                }
            }
        }

        for (int i = -1; i <= 2; i++)
        {
            Tile below = Framing.GetTileSafely(x + i, y + 2);
            if (below.HasTile && Main.tileSolid[below.TileType])
            {
                return true;
            }
        }

        return false;
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