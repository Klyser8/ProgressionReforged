using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee.DamageForUseTime;

public class HeftyPrefix() : LeveledPrefix(1, "damageForUseTime")
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
        damageMult = 1.30f; // 30% damage
        scaleMult = 1.09f; // +9% scale
        useTimeMult = 1.30f; // -30% use time
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
        return ModContent.PrefixType<CrushingPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<DensePrefix>();
    }
}