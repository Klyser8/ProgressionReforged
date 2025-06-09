using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged.UseTimeForVelocity;

public class DrawnPrefix() : LeveledPrefix(0, "useTimeForVelocity")
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
        shootSpeedMult = 0.82f; // -18% shoot speed
        useTimeMult = 0.87f; // -13% use time
        damageMult = 0.93f; // -7% damage
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
        return ModContent.PrefixType<TensedPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<JammedPrefix>();
    }
}