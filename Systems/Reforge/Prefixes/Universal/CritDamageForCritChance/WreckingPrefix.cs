using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChanceForCritDamage;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamageForCritChance;

public class WreckingPrefix() : CritDamagePrefix(2, "critDamageForCritChance")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
        ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        critBonus = -15; // -15% crit chance
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override float CritDamageMult => 1.66f; // 66% increased crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<FatalPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<PunishingPrefix>();
    }
}