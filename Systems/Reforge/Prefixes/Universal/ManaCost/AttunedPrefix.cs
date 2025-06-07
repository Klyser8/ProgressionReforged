using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;

public class AttunedPrefix() : LeveledPrefix(1, "manaCost")
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
        manaMult = 0.78f; // 22% less mana cost
    }
    
    public override int GetNext()
    {
        return ModContent.GetInstance<FocusedPrefix>().Type; // 2
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<ControlledPrefix>().Type; // -1
    }
}