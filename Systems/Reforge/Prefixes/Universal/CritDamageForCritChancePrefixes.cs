using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class DullPrefix() : SimpleCritDamagePrefix(
    -1,
    "critDamageForCritChance",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.02f, // +2% crit damage
    critBonus: -4, // -4% crit chance
    next: () => ModContent.GetInstance<RoughPrefix>().Type,
    previous: () => -1);
    
public class RoughPrefix() : SimpleCritDamagePrefix(
    0,
    "critDamageForCritChance",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.15f, // +15% crit damage
    critBonus: -4, // -4% crit chance
    next: () => ModContent.GetInstance<PunishingPrefix>().Type,
    previous: () => ModContent.GetInstance<DullPrefix>().Type);
    
public class PunishingPrefix() : SimpleCritDamagePrefix(
    1,
    "critDamageForCritChance",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.33f, // +33% crit damage
    critBonus: -8, // -8% crit chance
    next: () => ModContent.GetInstance<WreckingPrefix>().Type,
    previous: () => ModContent.GetInstance<RoughPrefix>().Type);
    
public class WreckingPrefix() : SimpleCritDamagePrefix(
    2,
    "critDamageForCritChance",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.66f, // +66% crit damage
    critBonus: -15, // -15% crit chance
    next: () => ModContent.GetInstance<FatalPrefix>().Type,
    previous: () => ModContent.GetInstance<PunishingPrefix>().Type);
    
public class FatalPrefix() : SimpleCritDamagePrefix(
    3,
    "critDamageForCritChance",
    PrefixCategory.AnyWeapon,
    critDamageMult: 2.11f, // +111% crit damage
    critBonus: -25, // -25% crit chance
    next: () => -1,
    previous: () => ModContent.GetInstance<WreckingPrefix>().Type);