using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public class WoundingPrefix() : CritDamagePrefix(1, "critDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override float CritDamageMult => 1.20f; // 20% more crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<ViciousPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<PrecisePrefix>();
    }
}