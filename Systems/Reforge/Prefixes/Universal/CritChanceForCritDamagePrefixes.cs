using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class FlimsyPrefix() : SimpleCritDamagePrefix(
    -1,
    "critChanceForCritDamage",
    PrefixCategory.AnyWeapon,
    critBonus: -1, // -1% crit chance
    critDamageMult: 0.88f, // -12% crit damage
    next: () => ModContent.GetInstance<TickingPrefix>().Type,
    previous: () => -1);
    
public class TickingPrefix() : SimpleCritDamagePrefix(
    0,
    "critChanceForCritDamage",
    PrefixCategory.AnyWeapon,
    critBonus: 5, // +5% crit chance
    critDamageMult: 0.90f, // -10% crit damage
    next: () => ModContent.GetInstance<PricklingPrefix>().Type,
    previous: () => ModContent.GetInstance<FlimsyPrefix>().Type);
    
public class PricklingPrefix() : SimpleCritDamagePrefix(
    1,
    "critChanceForCritDamage",
    PrefixCategory.AnyWeapon,
    critBonus: 13, // +13% crit chance
    critDamageMult: 0.85f, // -15% crit damage
    next: () => ModContent.GetInstance<NeedlingPrefix>().Type,
    previous: () => ModContent.GetInstance<TickingPrefix>().Type);
    
public class NeedlingPrefix() : SimpleCritDamagePrefix(
    2,
    "critChanceForCritDamage",
    PrefixCategory.AnyWeapon,
    critBonus: 22, // +22% crit chance
    critDamageMult: 0.79f, // -21% crit damage
    next: () => ModContent.GetInstance<CalibratedPrefix>().Type,
    previous: () => ModContent.GetInstance<PricklingPrefix>().Type);
    
public class CalibratedPrefix() : SimpleCritDamagePrefix(
    3,
    "critChanceForCritDamage",
    PrefixCategory.AnyWeapon,
    critBonus: 33, // +33% crit chance
    critDamageMult: 0.70f, // -30% crit damage
    next: () => -1,
    previous: () => ModContent.GetInstance<NeedlingPrefix>().Type);