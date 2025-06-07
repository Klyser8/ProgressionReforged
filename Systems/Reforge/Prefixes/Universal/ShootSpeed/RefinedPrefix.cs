using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ShootSpeed;

public class RefinedPrefix() : LeveledPrefix(0, "shootSpeed")
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
        shootSpeedMult = 1.10f; // 10% faster shoot speed
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<AcceleratedPrefix>().Type; // +1
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<HamperedPrefix>().Type; // -1
    }
}