using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Damage;

public class TemperedPrefix() : LeveledPrefix(0, "damage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        damageMult = 1.05f; // +5% damage
    }

    public override int GetNext()
    {
        return ModContent.PrefixType<FiercePrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<WeakPrefix>();
    }
}