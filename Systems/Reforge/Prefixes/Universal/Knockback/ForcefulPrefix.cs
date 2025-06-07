using ProgressionReforged.Systems.Reforge.Prefixes.Universal.Knockback;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class ForcefulPrefix() : LeveledPrefix(1, "knockback")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        knockbackMult *= 1.3f; // Increase knockback by 30%
    }
    
    public override int GetNext()
    {
        return ModContent.PrefixType<PowerfulPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<RudePrefix>();
    }
}