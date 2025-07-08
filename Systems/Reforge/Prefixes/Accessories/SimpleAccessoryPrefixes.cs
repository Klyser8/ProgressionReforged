using System;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public abstract class SimpleAccessoryPrefixes(
    int level,
    string chainKey,
    int defenseBonus = 0,
    int healthBonus = 0,
    int critBonus = 0,
    int armorPenBonus = 0,
    float critDamageMult = 1f,
    float jumpHeightMult = 1f,
    float knockbackResistMult = 1f,
    float damageMult = 1f,
    float manaRegenMult = 1f,
    float movementSpeedMult = 1f,
    Func<int>? next = null,
    Func<int>? previous = null)
    : AccessoryPrefix(level, chainKey)
{
    private readonly Func<int> _next = next ?? (() => -1);
    private readonly Func<int> _previous = previous ?? (() => -1);

    public override int DefenseBonus => defenseBonus;
    public override int HealthBonus => healthBonus;
    public override int CritBonus => critBonus;
    public override int ArmorPenBonus => armorPenBonus;
    public override float CritDamageMult => critDamageMult;
    public override float JumpHeightMult => jumpHeightMult;
    public override float KnockbackMult => knockbackResistMult;
    public override float DamageMult => damageMult;
    public override float ManaRegenMult => manaRegenMult;
    public override float MovementSpeedMult => movementSpeedMult;

    public override int GetNext() => _next();
    public override int GetPrevious() => _previous();
}