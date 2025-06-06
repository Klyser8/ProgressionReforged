using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Damage;

public class BrutalPrefix() : DamagePrefix(2, "damage")
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
        damageMult = 1.25f; // +25% damage
    }

    public override int GetNext()
    {
        return ModContent.PrefixType<SavagePrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<FiercePrefix>();
    }
}