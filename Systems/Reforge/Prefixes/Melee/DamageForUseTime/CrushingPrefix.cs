using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee.DamageForUseTime;

public class CrushingPrefix() : LeveledPrefix(2, "damageForUseTime")
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
        damageMult = 1.48f; // 48% damage
        scaleMult = 1.18f; // +18% scale
        useTimeMult = 1.48f; // -48% use time
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
        return ModContent.PrefixType<EarthshakingPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<HeftyPrefix>();
    }
}