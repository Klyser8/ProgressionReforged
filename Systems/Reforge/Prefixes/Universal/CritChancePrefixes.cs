using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class UnluckyPrefix() : SimpleCritChancePrefix(
    -1,
    "critChance",
    PrefixCategory.AnyWeapon,
    critBonus: -2, // -2% crit chance
    next: () => ModContent.GetInstance<SteadyPrefix>().Type,
    previous: () => -1);

public class SteadyPrefix() : SimpleCritChancePrefix(
    0,
    "critChance",
    PrefixCategory.AnyWeapon,
    critBonus: 2, // 2% crit chance
    next: () => ModContent.GetInstance<SharpPrefix>().Type,
    previous: () => ModContent.GetInstance<UnluckyPrefix>().Type);
    
public class SharpPrefix() : SimpleCritChancePrefix(
    1,
    "critChance",
    PrefixCategory.AnyWeapon,
    critBonus: 5, // 5% crit chance
    next: () => ModContent.GetInstance<KeenPrefix>().Type,
    previous: () => ModContent.GetInstance<SteadyPrefix>().Type);
    
public class KeenPrefix() : SimpleCritChancePrefix(
    2,
    "critChance",
    PrefixCategory.AnyWeapon,
    critBonus: 10, // 10% crit chance
    next: () => ModContent.GetInstance<ZealousPrefix>().Type,
    previous: () => ModContent.GetInstance<SharpPrefix>().Type);
    
public class ZealousPrefix() : SimpleCritChancePrefix(
    3,
    "critChance",
    PrefixCategory.AnyWeapon,
    critBonus: 18, // 18% crit chance
    next: () => -1,
    previous: () => ModContent.GetInstance<KeenPrefix>().Type);