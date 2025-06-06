using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Damage;

public class WeakPrefix() : DamagePrefix(-1, "damage")
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
        damageMult = 0.9f; // -10% damage
    }

    public override int GetNext()
    {
        return ModContent.PrefixType<TemperedPrefix>();
    }

    public override int GetPrevious()
    {
        return -1; // No previous prefix, as this is the lowest level
    }
}