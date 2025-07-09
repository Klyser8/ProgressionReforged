using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public abstract class AccessoryPrefix(int level, string chainKey)
    : LeveledPrefix(level, chainKey), IAccessoryPrefixProvider
{
    public abstract int DefenseBonus { get; }
    public abstract int HealthBonus { get; }
    public abstract int CritBonus { get; }
    public abstract int ArmorPenBonus { get; }
    public abstract float CritDamageMult { get; }
    public abstract float JumpHeightMult { get; }
    public abstract float KnockbackMult { get; }
    public abstract float DamageMult { get; }
    public abstract float ManaRegenMult { get; }
    public abstract float MovementSpeedMult { get; }

    // internal fields for price calculation
    public int DefenseBonusInternal { get; private set; }
    public int HealthBonusInternal { get; private set; }
    public int CritBonusInternal { get; private set; }
    public int ArmorPenBonusInternal { get; private set; }
    public float CritDamageMultInternalAcc { get; private set; } = 1f;
    public float JumpHeightMultInternal { get; private set; } = 1f;
    public float KnockbackMultInternal { get; private set; } = 1f;
    public float DamageMultInternal { get; private set; } = 1f;
    public float ManaRegenMultInternal { get; private set; } = 1f;
    public float MovementSpeedMultInternal { get; private set; } = 1f;

    public override PrefixCategory Category => PrefixCategory.Accessory;

    public override bool CanRoll(Item item) => item.accessory;

    public override void SetStats(ref float damageMult, ref float knockbackMult,
        ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        DefenseBonusInternal = DefenseBonus;
        HealthBonusInternal = HealthBonus;
        CritBonusInternal = CritBonus;
        ArmorPenBonusInternal = ArmorPenBonus;
        CritDamageMultInternalAcc = CritDamageMult;
        JumpHeightMultInternal = JumpHeightMult;
        KnockbackMultInternal = KnockbackMult;
        DamageMultInternal = DamageMult;
        ManaRegenMultInternal = ManaRegenMult;
        MovementSpeedMultInternal = MovementSpeedMult;
        damageMult = 1f;
        knockbackMult = 1f;
        useTimeMult = 1f;
        scaleMult = 1f;
        shootSpeedMult = 1f;
        manaMult = 1f;
        critBonus = 0;
        base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult,
            ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
    }

    public static LocalizedText DefenseTooltip { get; private set; }
    public static LocalizedText HealthTooltip { get; private set; }
    public static LocalizedText CritChanceTooltip { get; private set; }
    public static LocalizedText ArmorPenTooltip { get; private set; }
    public static LocalizedText CritDamageTooltipAcc { get; private set; }
    public static LocalizedText JumpHeightTooltip { get; private set; }
    public static LocalizedText KnockbackResistTooltip { get; private set; }
    public static LocalizedText DamageTooltipAcc { get; private set; }
    public static LocalizedText ManaRegenTooltip { get; private set; }
    public static LocalizedText MovementSpeedTooltip { get; private set; }

    public LocalizedText AdditionalTooltip => this.GetLocalization(nameof(AdditionalTooltip));

    public override void SetStaticDefaults()
    {
        DefenseTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(DefenseTooltip)}");
        HealthTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(HealthTooltip)}");
        CritChanceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(CritChanceTooltip)}");
        ArmorPenTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ArmorPenTooltip)}");
        CritDamageTooltipAcc = Mod.GetLocalization($"{LocalizationCategory}.{nameof(CritDamageTooltipAcc)}");
        JumpHeightTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(JumpHeightTooltip)}");
        KnockbackResistTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(KnockbackResistTooltip)}");
        DamageTooltipAcc = Mod.GetLocalization($"{LocalizationCategory}.{nameof(DamageTooltipAcc)}");
        ManaRegenTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ManaRegenTooltip)}");
        MovementSpeedTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(MovementSpeedTooltip)}");
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        if (DefenseBonus != 0)
            yield return new TooltipLine(Mod, "PrefixDefense", DefenseTooltip.Format(DefenseBonus)) { IsModifier = true, IsModifierBad = DefenseBonus < 0 };
        if (HealthBonus != 0)
            yield return new TooltipLine(Mod, "PrefixHealth", HealthTooltip.Format(HealthBonus)) { IsModifier = true, IsModifierBad = HealthBonus < 0 };
        if (CritBonus != 0)
            yield return new TooltipLine(Mod, "PrefixCrit", CritChanceTooltip.Format(CritBonus)) { IsModifier = true, IsModifierBad = CritBonus < 0 };
        if (ArmorPenBonus != 0)
            yield return new TooltipLine(Mod, "PrefixArmorPen", ArmorPenTooltip.Format(ArmorPenBonus)) { IsModifier = true, IsModifierBad = ArmorPenBonus < 0 };
        if (Math.Abs(CritDamageMult - 1f) > 0.001f)
            yield return new TooltipLine(Mod, "PrefixCritDamageAcc", CritDamageTooltipAcc.Format((int)MathF.Round((CritDamageMult - 1f) * 100f))) { IsModifier = true, IsModifierBad = CritDamageMult < 1f };
        if (Math.Abs(JumpHeightMult - 1f) > 0.001f)
            yield return new TooltipLine(Mod, "PrefixJumpHeight", JumpHeightTooltip.Format((int)MathF.Round((JumpHeightMult - 1f) * 100f))) { IsModifier = true, IsModifierBad = JumpHeightMult < 1f };
        if (Math.Abs(KnockbackMult - 1f) > 0.001f)
            yield return new TooltipLine(Mod, "PrefixKnockbackResist", KnockbackResistTooltip.Format((int)MathF.Round((1f - KnockbackMult) * 100f))) { IsModifier = true, IsModifierBad = KnockbackMult > 1f };
        if (Math.Abs(DamageMult - 1f) > 0.001f)
            yield return new TooltipLine(Mod, "PrefixAccessoryDamage", DamageTooltipAcc.Format((int)MathF.Round((DamageMult - 1f) * 100f))) { IsModifier = true, IsModifierBad = DamageMult < 1f };
        if (Math.Abs(ManaRegenMult - 1f) > 0.001f)
            yield return new TooltipLine(Mod, "PrefixManaRegen", ManaRegenTooltip.Format((int)MathF.Round((ManaRegenMult - 1f) * 100f))) { IsModifier = true, IsModifierBad = ManaRegenMult < 1f };
        if (Math.Abs(MovementSpeedMult - 1f) > 0.001f)
            yield return new TooltipLine(Mod, "PrefixMovementSpeed", MovementSpeedTooltip.Format((int)MathF.Round((MovementSpeedMult - 1f) * 100f))) { IsModifier = true, IsModifierBad = MovementSpeedMult < 1f };
    }
}