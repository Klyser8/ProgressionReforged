using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Knockback;

public class FeeblePrefix() : LeveledPrefix(-1, "knockback")
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
        knockbackMult *= 0.8f; // Reduce knockback by 20%
    }
    
    public override int GetNext()
    {
        return ModContent.PrefixType<RudePrefix>();
    }

    public override int GetPrevious()
    {
        return -1;
    }
}