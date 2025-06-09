using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChanceForCritDamage;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamageForCritChance;

public class FatalPrefix() : CritDamagePrefix(3, "critDamageForCritChance")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        critBonus = -25; // -25% crit chance
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override float CritDamageMult => 2.11f; // 111% increased crit damage
    
    public override int GetNext()
    {
        return -1;
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<WreckingPrefix>();
    }
}