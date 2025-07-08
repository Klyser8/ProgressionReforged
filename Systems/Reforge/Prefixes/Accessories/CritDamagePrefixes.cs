using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class MercifulPrefixes() : SimpleAccessoryPrefixes(
    level: -1,
    "acc.critDamage",
    critDamageMult: 0.985f, // -1.5% crit damage
    next: () => ModContent.GetInstance<SharpenedPrefixes>().Type,
    previous: () => -1);
    
public class SharpenedPrefixes() : SimpleAccessoryPrefixes(
    level: 0,
    "acc.critDamage",
    critDamageMult: 1.015f, // +1.5% crit damage
    next: () => ModContent.GetInstance<CunningPrefixes>().Type,
    previous: () => ModContent.GetInstance<MercifulPrefixes>().Type);
    
public class CunningPrefixes() : SimpleAccessoryPrefixes(
    level: 1,
    "acc.critDamage",
    critDamageMult: 1.03f, // +3% crit damage
    next: () => ModContent.GetInstance<CruelPrefixes>().Type,
    previous: () => ModContent.GetInstance<SharpenedPrefixes>().Type);
    
public class CruelPrefixes() : SimpleAccessoryPrefixes(
    level: 2,
    "acc.critDamage",
    critDamageMult: 1.045f, // +4.5% crit damage
    next: () => ModContent.GetInstance<RuthlessPrefixes>().Type,
    previous: () => ModContent.GetInstance<CunningPrefixes>().Type);
    
public class RuthlessPrefixes() : SimpleAccessoryPrefixes(
    level: 3,
    "acc.critDamage",
    critDamageMult: 1.06f, // +6% crit damage
    next: () => -1,
    previous: () => ModContent.GetInstance<CruelPrefixes>().Type);