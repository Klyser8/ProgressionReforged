using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class SteadyPrefix() : LeveledPrefix(0, "critChance")
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
        critBonus = 2; // 2% crit chance
    }
    
    public override int GetNext()
    {
        return ModContent.PrefixType<SharpPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<UnluckyPrefix>();
    }
}