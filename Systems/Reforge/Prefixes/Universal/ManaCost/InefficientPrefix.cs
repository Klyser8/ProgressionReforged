using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;

public class InefficientPrefix() : LeveledPrefix(-1, "manaCost")
{
    public override PrefixCategory Category => PrefixCategory.Magic;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        manaMult = 1.13f; // 13% more mana cost
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<ControlledPrefix>().Type; // 0
    }

    public override int GetPrevious()
    {
        return -1;
    }
}