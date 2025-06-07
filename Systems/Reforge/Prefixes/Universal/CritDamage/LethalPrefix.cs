using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritChance;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public class LethalPrefix() : CritDamagePrefix(3, "critDamage")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
    public override float CritDamageMult => 1.5f; // 50% more crit damage
    
    public override int GetNext()
    {
        return -1;
    }

    public override int GetPrevious()
    {
        return ModContent.PrefixType<ViciousPrefix>();
    }
}