using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged.UseTimeForVelocity;

public class BallisticPrefix() : LeveledPrefix(3, "useTimeForVelocity")
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
        shootSpeedMult = 0.31f; // -69% shoot speed
        useTimeMult = 0.50f; // -50% use time
        damageMult = 0.75f; // -25% damage
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
        return ModContent.PrefixType<PropelledPrefix>();
    }
}