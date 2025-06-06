using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Damage;

public class FiercePrefix() : DamagePrefix(1, "damage")
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
        damageMult = 1.15f; // +15% damage
        base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
    }

    public override int GetNext()
    {
        return ModContent.PrefixType<BrutalPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<TemperedPrefix>();
    }
}