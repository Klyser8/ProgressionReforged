using System.Collections.Generic;

namespace ProgressionReforged.Systems.Hooks;

internal static class HookStatData
{
    // 1) Single-type hooks:
    //    Key = (oldRange, projectileID), Value = newRange (in pixels)
    internal static readonly Dictionary<(float oldRange, int projID), float> SingleHookRanges = new()
    {
        { (350f, 256), 20f * 16f }, // Skeletron Hand
        { (400f, 372), 19f * 16f }, // Fish Hook
        { (300f, 865), 21f * 16f }, // Squirrel Hook
        { (440f, 73),  26f * 16f }, // Dual Hook (red)
        { (440f, 74),  26f * 16f }, // Dual Hook (blue)
        { (400f, 32),  22f * 16f }, // Ivy Whip
        { (500f, 935), 23f * 16f }, // Dissonance Hook
        { (550f, 332), 29f * 16f }, // Christmas Hook
        { (550f, 322), 29f * 16f }, // Bat Hook
    };

    // 2) Gem hooks (230..235) => vanilla does: num8 = 300 + (type - 230) * 30
    internal static readonly Dictionary<int, int> GemHookRanges = new()
    {
        [230] = (int)(19.5f * 16f), // Amethyst
        [231] = (int)(20.25f * 16f),
        [232] = (int)(21.25f * 16f),
        [233] = (int)(22.25f * 16f),
        [234] = (int)(23.5f * 16f),
        [235] = (int)(25f * 16f),
    };

    // 3) Amber Hook => if (type == 753) { int num9 = 420; if (num3 > (float)num9) ... }
    internal const int AmberVanilla = 420;
    internal const int AmberNew     = (int)(23f * 16f); // 368 pixels

    // 4) Multi-type block for Tendon (488), Illuminant (486), Worm (489), Thorn (487).
    internal static readonly Dictionary<int, float> MultiHookRanges = new()
    {
        [486] = 24f * 16f, // Illuminant Hook
        [487] = 27f * 16f, // Thorn Hook
        [488] = 24f * 16f, // Tendon Hook
        [489] = 24f * 16f, // Worm Hook
    };

    internal static bool TryGetCustomRangePixels(int projectileType, out float rangePixels)
    {
        foreach (var ((_, projId), newRange) in SingleHookRanges)
        {
            if (projId == projectileType)
            {
                rangePixels = newRange;
                return true;
            }
        }

        if (GemHookRanges.TryGetValue(projectileType, out int gemRange))
        {
            rangePixels = gemRange;
            return true;
        }

        if (projectileType == 753) // Amber Hook
        {
            rangePixels = AmberNew;
            return true;
        }

        if (MultiHookRanges.TryGetValue(projectileType, out float multiRange))
        {
            rangePixels = multiRange;
            return true;
        }

        rangePixels = 0f;
        return false;
    }
}