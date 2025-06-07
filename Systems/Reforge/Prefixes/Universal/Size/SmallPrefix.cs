using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class SmallPrefix() : LeveledPrefix(-1, "size")
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
        scaleMult = 0.85f; // 15% smaller
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<AboveAveragePrefix>().Type;
    }

    public override int GetPrevious()
    {
        return -1;
    }
}