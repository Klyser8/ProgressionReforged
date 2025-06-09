using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ShootSpeed;

public class AcceleratedPrefix() : LeveledPrefix(1, "shootSpeed")
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
        shootSpeedMult = 1.30f; // 30% faster shoot speed
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
        return ModContent.GetInstance<SonicPrefix>().Type; // +1
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<RefinedPrefix>().Type; // -1
    }
}