using ProgressionReforged.Systems.Reforge.Prefixes.Universal.ManaCost;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic.DamageForManaCost;

public class UnboundPrefix() : LeveledPrefix(3, "damageForManaCost")
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
        manaMult = 4.00f; // +300% mana cost
        damageMult = 1.45f; // +45% damage
        useTimeMult = 0.72f; // -28% use time
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
        return ModContent.GetInstance<SurgingPrefix>().Type;
    }
}