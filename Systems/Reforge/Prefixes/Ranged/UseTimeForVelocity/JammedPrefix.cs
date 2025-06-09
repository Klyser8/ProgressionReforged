using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged.UseTimeForVelocity;

public class JammedPrefix() : LeveledPrefix(-1, "useTimeForVelocity")
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
        shootSpeedMult = 0.93f; // -7% shoot speed
        useTimeMult = 0.95f; // -5% use time
        damageMult = 0.97f; // -3% damage
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
        return ModContent.PrefixType<DrawnPrefix>();
    }

    public override int GetPrevious()
    {
        return -1;
    }
}