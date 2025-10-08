using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Hooks;

public class HookTooltipGlobalItem : GlobalItem
{
    private static readonly float[] GrappleRanges = TryGetProjectileSet<float>("GrappleRange", "GrappleRanges", "HookRange");
    private static readonly float[] GrapplePullSpeeds = TryGetProjectileSet<float>("GrapplePullSpeed", "GrapplePullSpeeds");
    private static readonly float[] GrappleLaunchSpeeds = TryGetProjectileSet<float>("GrappleThrowSpeed", "GrappleLaunchSpeed", "GrappleThrowSpeeds");
    private static readonly int[] GrappleHookCounts = TryGetProjectileSet<int>("GrappleNumHooks", "GrappleHookCounts", "GrappleHookNumbers");

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item.shoot <= ProjectileID.None || item.shoot >= ProjectileLoader.ProjectileCount)
            return;

        if (!Main.projHook[item.shoot])
            return;

        List<TooltipLine> hookLines = BuildHookTooltipLines(item);
        if (hookLines.Count == 0)
            return;

        int insertIndex = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "Speed");
        if (insertIndex < 0)
            insertIndex = tooltips.Count;

        tooltips.InsertRange(insertIndex, hookLines);
    }

    private static List<TooltipLine> BuildHookTooltipLines(Item item)
    {
        var lines = new List<TooltipLine>();

        if (TryGetRangeTiles(item.shoot, out float rangeTiles))
        {
            lines.Add(CreateLine(item, "HookRange", $"Reach: {rangeTiles:0.#} tiles"));
        }

        float launchTilesPerSecond = GetLaunchTilesPerSecond(item);
        if (launchTilesPerSecond > 0f)
        {
            lines.Add(CreateLine(item, "HookLaunchSpeed", $"Launch Speed: {launchTilesPerSecond:0.#} tiles/sec"));
        }

        if (TryGetPullTilesPerSecond(item.shoot, out float pullTilesPerSecond) && pullTilesPerSecond > 0f)
        {
            lines.Add(CreateLine(item, "HookPullSpeed", $"Pull Speed: {pullTilesPerSecond:0.#} tiles/sec"));
        }

        if (TryGetHookCount(item.shoot, out int hookCount) && hookCount > 0)
        {
            string plural = hookCount == 1 ? "hook" : "hooks";
            lines.Add(CreateLine(item, "HookCount", $"Can latch {hookCount} {plural}"));
        }

        return lines;
    }

    private static TooltipLine CreateLine(Item item, string name, string text)
    {
        return new TooltipLine(ModContent.GetInstance<ProgressionReforged>(), name, text);
    }

    private static bool TryGetRangeTiles(int projectileType, out float rangeTiles)
    {
        if (HookStatData.TryGetCustomRangePixels(projectileType, out float customRangePixels))
        {
            rangeTiles = customRangePixels / 16f;
            return true;
        }

        float? fromArray = TryGetArrayValue(GrappleRanges, projectileType);
        if (fromArray.HasValue)
        {
            float value = fromArray.Value;
            rangeTiles = value > 60f ? value / 16f : value;
            return rangeTiles > 0f;
        }

        rangeTiles = 0f;
        return false;
    }

    private static bool TryGetPullTilesPerSecond(int projectileType, out float pullTilesPerSecond)
    {
        float? value = TryGetArrayValue(GrapplePullSpeeds, projectileType);
        if (value.HasValue && value.Value > 0f)
        {
            pullTilesPerSecond = value.Value * 60f / 16f;
            return true;
        }

        pullTilesPerSecond = 0f;
        return false;
    }

    private static bool TryGetHookCount(int projectileType, out int count)
    {
        int? value = TryGetArrayValue(GrappleHookCounts, projectileType);
        if (value.HasValue && value.Value > 0)
        {
            count = value.Value;
            return true;
        }

        count = 0;
        return false;
    }

    private static float GetLaunchTilesPerSecond(Item item)
    {
        float? value = TryGetArrayValue(GrappleLaunchSpeeds, item.shoot);
        float speedPixelsPerFrame = value.HasValue && value.Value > 0f ? value.Value : item.shootSpeed;
        return speedPixelsPerFrame * 60f / 16f;
    }

    private static T[] TryGetProjectileSet<T>(params string[] possibleNames) where T : struct
    {
        Type elementType = typeof(T);
        Type? setsType = typeof(ProjectileID).GetNestedType("Sets", BindingFlags.Public | BindingFlags.NonPublic);
        if (setsType is null)
            return Array.Empty<T>();

        foreach (string name in possibleNames)
        {
            FieldInfo? field = setsType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field is null || !field.FieldType.IsArray || field.FieldType.GetElementType() != elementType)
                continue;

            if (field.GetValue(null) is T[] typedArray)
                return typedArray;
        }

        // fall back to the first array field that loosely matches the element type and name contains "Grapple"
        FieldInfo? fallback = setsType
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .FirstOrDefault(f => f.FieldType.IsArray && f.FieldType.GetElementType() == elementType && f.Name.Contains("Grapple", StringComparison.OrdinalIgnoreCase));

        if (fallback?.GetValue(null) is T[] fallbackArray)
            return fallbackArray;

        return Array.Empty<T>();
    }

    private static float? TryGetArrayValue(float[] values, int index)
    {
        if (values.Length > index)
            return values[index];

        return null;
    }

    private static int? TryGetArrayValue(int[] values, int index)
    {
        if (values.Length > index)
            return values[index];

        return null;
    }
}
