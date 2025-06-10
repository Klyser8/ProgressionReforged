using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class SoftHittingPrefix() : SimpleCritDamagePrefix(
    -1,
    "critDamage",
    PrefixCategory.AnyWeapon,
    critDamageMult: 0.88f, // -12% crit damage
    next: () => ModContent.GetInstance<SteadyPrefix>().Type,
    previous: () => -1);

public class PrecisePrefix() : SimpleCritDamagePrefix(
    0,
    "critDamage",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.08f, // +8% crit damage
    next: () => ModContent.GetInstance<WoundingPrefix>().Type,
    previous: () => ModContent.GetInstance<SoftHittingPrefix>().Type);

public class WoundingPrefix() : SimpleCritDamagePrefix(
    1,
    "critDamage",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.20f, // +20% crit damage
    next: () => ModContent.GetInstance<ViciousPrefix>().Type,
    previous: () => ModContent.GetInstance<PrecisePrefix>().Type);

public class ViciousPrefix() : SimpleCritDamagePrefix(
    2,
    "critDamage",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.33f, // +33% crit damage
    next: () => ModContent.GetInstance<LethalPrefix>().Type,
    previous: () => ModContent.GetInstance<WoundingPrefix>().Type);

public class LethalPrefix() : SimpleCritDamagePrefix(
    3,
    "critDamage",
    PrefixCategory.AnyWeapon,
    critDamageMult: 1.50f, // +50% crit damage
    next: () => -1,
    previous: () => ModContent.GetInstance<ViciousPrefix>().Type);
