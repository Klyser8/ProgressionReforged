using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ShootSpeed;

public class HamperedPrefix() : LeveledPrefix(-1, "shootSpeed")
{
    public override PrefixCategory Category => PrefixCategory.Ranged;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        shootSpeedMult = 0.85f; // 15% less shoot speed
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
        return ModContent.PrefixType<RefinedPrefix>();
    }

    public override int GetPrevious()
    {
        return -1;
    }
}