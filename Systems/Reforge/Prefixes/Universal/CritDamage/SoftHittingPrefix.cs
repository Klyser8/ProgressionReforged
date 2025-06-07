using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public class SoftHittingPrefix() : CritDamagePrefix(-1, "critDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override float CritDamageMult => 0.75f; // 25% less crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<PrecisePrefix>();
    }

    public override int GetPrevious()
    {
        return -1;
    }
}