using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;

public class MysticPrefix() : LeveledPrefix(3, "manaCost")
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
        manaMult = 0.50f; // 50% less mana cost
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
        return -1;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<AttunedPrefix>().Type; // -1
    }
}