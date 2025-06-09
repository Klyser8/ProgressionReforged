using ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic.DamageForManaCost;

public class SurgingPrefix() : LeveledPrefix(2, "damageForManaCost")
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
        manaMult = 3.00f; // +200% mana cost
        damageMult = 1.32f; // +32% damage
        useTimeMult = 0.78f; // -22% use time
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
        return ModContent.GetInstance<UnboundPrefix>().Type;
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<BurstingPrefix>().Type;
    }
}