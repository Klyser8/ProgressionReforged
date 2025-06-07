using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class BigPrefix() : LeveledPrefix(1, "size")
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
        scaleMult = 1.15f; // 15% bigger
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<MassivePrefix>().Type;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<AboveAveragePrefix>().Type;
    }
}