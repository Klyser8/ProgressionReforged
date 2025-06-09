using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChanceForCritDamage;

public class NeedlingPrefix() : CritDamagePrefix(2, "critChanceForCritDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        critBonus = 22; // +22% crit chance
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override float CritDamageMult => 0.79f; // 21% less crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<CalibratedPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<PricklingPrefix>();
    }
}