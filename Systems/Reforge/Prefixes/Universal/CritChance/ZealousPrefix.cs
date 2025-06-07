using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class ZealousPrefix() : LeveledPrefix(3, "critChance")
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
        critBonus = 18; // 18% crit chance
    }
    
    public override int GetNext()
    {
        return -1; // No next prefix, as this is the highest level
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<KeenPrefix>();
    }
}