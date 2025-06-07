using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;

public class FocusedPrefix() : LeveledPrefix(2, "manaCost")
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
        manaMult = 0.67f; // 33% less mana cost
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<MysticPrefix>().Type; // 3
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<AttunedPrefix>().Type; // -1
    }
}