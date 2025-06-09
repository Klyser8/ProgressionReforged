using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChanceForCritDamage;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamageForCritChance;

public class PunishingPrefix() : CritDamagePrefix(1, "critDamageForCritChance")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        critBonus = -8; // -8% crit chance
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override float CritDamageMult => 1.33f; // 33% increased crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<WreckingPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<RoughPrefix>();
    }
}