using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public class PrecisePrefix() : CritDamagePrefix(0, "critDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override float CritDamageMult => 1.08f; // 8% more crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<WoundingPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<SoftHittingPrefix>();
    }
}