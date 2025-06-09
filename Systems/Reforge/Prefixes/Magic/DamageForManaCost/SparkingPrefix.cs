using ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic.DamageForManaCost;

public class SparkingPrefix() : LeveledPrefix(0, "damageForManaCost")
{
    public override PrefixCategory Category => PrefixCategory.Magic;
    
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        manaMult = 1.550f; // +55% mana cost
        damageMult = 1.12f; // +12% damage
        useTimeMult = 0.90f; // -10% use time
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
        return ModContent.GetInstance<BurstingPrefix>().Type;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<VolatilePrefix>().Type;
    }
}