using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee.DamageForUseTime;

public class DensePrefix() : LeveledPrefix(0, "damageForUseTime")
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
        damageMult = 1.17f; // 17% damage
        scaleMult = 1.03f; // +3% scale
        useTimeMult = 1.17f; // -17% use time
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
        return ModContent.PrefixType<HeftyPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<UnwieldyPrefix>();
    }
}