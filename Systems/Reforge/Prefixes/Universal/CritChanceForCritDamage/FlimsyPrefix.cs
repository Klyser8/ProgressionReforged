using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChanceForCritDamage;

public class FlimsyPrefix() : CritDamagePrefix(-1, "critChanceForCritDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        critBonus = -1; // -1% crit chance
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override float CritDamageMult => 0.95f; // 5% less crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<TickingPrefix>();
    }

    public override int GetPrevious()
    {
        return -1;
    }
}