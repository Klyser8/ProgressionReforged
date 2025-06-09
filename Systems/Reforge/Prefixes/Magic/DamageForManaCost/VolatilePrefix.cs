using ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic.DamageForManaCost;

public class VolatilePrefix() : LeveledPrefix(-1, "damageForManaCost")
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
        manaMult = 1.20f; // +20% mana cost
        damageMult = 1.05f; // +5% damage
        useTimeMult = 0.95f; // -5% use time
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
        return ModContent.GetInstance<SparkingPrefix>().Type; // 0
    }

    public override int GetPrevious()
    {
        return -1;
    }
}