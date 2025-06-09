using ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic.DamageForManaCost;

public class BurstingPrefix() : LeveledPrefix(1, "damageForManaCost")
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
        manaMult = 2.20f; // +120% mana cost
        damageMult = 1.21f; // +21% damage
        useTimeMult = 0.84f; // -16% use time
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
        return ModContent.GetInstance<SurgingPrefix>().Type;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<SparkingPrefix>().Type;
    }
}