using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged.UseTimeForVelocity;

public class PropelledPrefix() : LeveledPrefix(2, "useTimeForVelocity")
{
    public override PrefixCategory Category => PrefixCategory.Ranged;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        shootSpeedMult = 0.53f; // -47% shoot speed
        useTimeMult = 0.64f; // -36% use time
        damageMult = 0.82f; // -18% damage
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
        return ModContent.PrefixType<BallisticPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<TensedPrefix>();
    }
}