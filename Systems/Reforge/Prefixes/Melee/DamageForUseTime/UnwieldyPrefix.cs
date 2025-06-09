using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee.DamageForUseTime;

public class UnwieldyPrefix() : LeveledPrefix(-1, "damageForUseTime")
{
    public override PrefixCategory Category => PrefixCategory.Melee;
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        damageMult = 1.07f; // +7% damage
        scaleMult = 0.95f; // -5% scale
        useTimeMult = 1.07f; // -7% use time
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
        return ModContent.PrefixType<DensePrefix>();
    }

    public override int GetPrevious()
    {
        return -1; // No previous prefix, as this is the lowest level
    }
}