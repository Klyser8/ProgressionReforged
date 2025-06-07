using ProgressionReforged.Systems.Reforge.Prefixes.Universal.Knockback;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;

public class OverwhelmingPrefix() : LeveledPrefix(3, "knockback")
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
        knockbackMult *= 1.66f; // Increase knockback by 66%
    }
    
    public override int GetNext()
    {
        return -1; // No next prefix, as this is the highest level
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<PowerfulPrefix>();
    }
}