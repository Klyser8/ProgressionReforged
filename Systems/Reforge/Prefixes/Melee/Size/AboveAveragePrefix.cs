using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class AboveAveragePrefix() : LeveledPrefix(0, "size")
{
    public override PrefixCategory Category => PrefixCategory.Melee;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        scaleMult = 1.05f; // 5% bigger
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<BigPrefix>().Type;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<SmallPrefix>().Type;
    }
}