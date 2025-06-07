using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ShootSpeed;

public class BlazingPrefix() : LeveledPrefix(3, "shootSpeed")
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
        shootSpeedMult = 2.00f; // 100% faster shoot speed
    }
    
    public override int GetNext()
    {
        return -1;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<SonicPrefix>().Type; // -1
    }
}