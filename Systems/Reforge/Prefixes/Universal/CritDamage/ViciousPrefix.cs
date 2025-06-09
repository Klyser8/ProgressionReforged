using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public class ViciousPrefix() : CritDamagePrefix(2, "critDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override float CritDamageMult => 1.33f; // 33% more crit damage
    
    public override int GetNext()
    {
        return ModContent.PrefixType<LethalPrefix>();
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<WoundingPrefix>();
    }
}