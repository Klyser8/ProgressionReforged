using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChanceForCritDamage;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamageForCritChance;

public class RoughPrefix() : CritDamagePrefix(0, "critDamageForCritChance")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        critBonus = -4; // -4% crit chance
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override float CritDamageMult => 1.15f; // 15% increased crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<PunishingPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<DullPrefix>();
    }
}