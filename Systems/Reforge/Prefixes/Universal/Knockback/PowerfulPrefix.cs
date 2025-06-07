using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Knockback;

public class PowerfulPrefix() : LeveledPrefix(2, "knockback")
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
        knockbackMult *= 1.45f; // Increase knockback by 45%
    }
    
    public override int GetNext()
    {
        return ModContent.PrefixType<OverwhelmingPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<ForcefulPrefix>();
    }
}