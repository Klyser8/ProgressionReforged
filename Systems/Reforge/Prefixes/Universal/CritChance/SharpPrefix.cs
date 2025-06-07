using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class SharpPrefix() : LeveledPrefix(1, "critChance")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        critBonus = 5; // 5% crit chance
    }
    
    public override int GetNext()
    {
        return ModContent.PrefixType<KeenPrefix>(); // Next prefix in the crit chance chain
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<SteadyPrefix>();
    }
}