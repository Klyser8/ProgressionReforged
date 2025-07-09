using System;
using System.Collections.Generic;
using ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

namespace ProgressionReforged.Systems.Reforge;

internal enum AccessoryStat {
    Defense,
    Health,
    CritChance,
    ArmorPen,
    CritDamage,
    JumpHeight,
    KnockbackResist,
    Damage,
    ManaRegen,
    MovementSpeed
}

internal static class AccessoryPriceConfig {
    private static readonly Dictionary<AccessoryStat, float> Weights = new()
    {
        [AccessoryStat.ArmorPen] = 0.75f,
        [AccessoryStat.Health] = 0.75f,
        [AccessoryStat.CritDamage] = 0.69f,
        [AccessoryStat.Defense] = 0.60f,
        [AccessoryStat.CritChance] = 0.60f,
        [AccessoryStat.KnockbackResist] = 0.54f,
        [AccessoryStat.Damage] = 0.54f,
        [AccessoryStat.ManaRegen] = 0.50f,
        [AccessoryStat.MovementSpeed] = 0.45f,
        [AccessoryStat.JumpHeight] = 0.45f
    };

    internal static float GetWeight(AccessoryPrefix prefix)
    {
        float weight = 0f;
        if (prefix.DefenseBonusInternal != 0)
            weight += Weights[AccessoryStat.Defense];
        if (prefix.HealthBonusInternal != 0)
            weight += Weights[AccessoryStat.Health];
        if (prefix.CritBonusInternal != 0)
            weight += Weights[AccessoryStat.CritChance];
        if (prefix.ArmorPenBonusInternal != 0)
            weight += Weights[AccessoryStat.ArmorPen];
        if (Math.Abs(prefix.CritDamageMultInternalAcc - 1f) > 0.001f)
            weight += Weights[AccessoryStat.CritDamage];
        if (Math.Abs(prefix.JumpHeightMultInternal - 1f) > 0.001f)
            weight += Weights[AccessoryStat.JumpHeight];
        if (Math.Abs(prefix.KnockbackMultInternal - 1f) > 0.001f)
            weight += Weights[AccessoryStat.KnockbackResist];
        if (Math.Abs(prefix.DamageMultInternal - 1f) > 0.001f)
            weight += Weights[AccessoryStat.Damage];
        if (Math.Abs(prefix.ManaRegenMultInternal - 1f) > 0.001f)
            weight += Weights[AccessoryStat.ManaRegen];
        if (Math.Abs(prefix.MovementSpeedMultInternal - 1f) > 0.001f)
            weight += Weights[AccessoryStat.MovementSpeed];
        return weight;
    }
}