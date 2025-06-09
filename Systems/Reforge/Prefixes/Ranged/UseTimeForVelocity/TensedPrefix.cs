using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged.UseTimeForVelocity;

public class TensedPrefix() : LeveledPrefix(1, "useTimeForVelocity")
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
        shootSpeedMult = 0.69f; // -31% shoot speed
        useTimeMult = 0.76f; // -24% use time
        damageMult = 0.88f; // -12% damage
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
        return ModContent.PrefixType<PropelledPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<DrawnPrefix>();
    }
}