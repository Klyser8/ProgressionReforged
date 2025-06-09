using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee.DamageForUseTime;

public class EarthshakingPrefix() : LeveledPrefix(3, "damageForUseTime")
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
        damageMult = 1.70f; // 70% damage
        scaleMult = 1.30f; // +30% scale
        useTimeMult = 1.70f; // -70% use time
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
        return -1;
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<CrushingPrefix>();
    }
}