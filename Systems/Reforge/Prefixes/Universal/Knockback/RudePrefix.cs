using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Knockback;

public class RudePrefix() : LeveledPrefix(0, "knockback")
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
        knockbackMult *= 1.15f; // Increase knockback by 15%
    }
    
    public override int GetNext()
    {
        return ModContent.PrefixType<ForcefulPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<FeeblePrefix>();
    }
}