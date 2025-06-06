using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Damage;

public class SavagePrefix() : DamagePrefix(3, "damage")
{
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        damageMult = 1.4f; // +40% damage
        base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
    }

    public override int GetNext()
    {
        return -1; // No next prefix, as this is the highest level
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<FiercePrefix>();
    }
}