using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class WeakPrefix() : SimpleLeveledPrefix(
    -1,
    "damage",
    PrefixCategory.AnyWeapon,
    damageMult: 0.90f, // -10% damage
    next: () => ModContent.GetInstance<TemperedPrefix>().Type,
    previous: () => -1);
    
public class TemperedPrefix() : SimpleLeveledPrefix(
    0,
    "damage",
    PrefixCategory.AnyWeapon,
    damageMult: 1.05f, // +5% damage
    next: () => ModContent.GetInstance<FiercePrefix>().Type,
    previous: () => ModContent.GetInstance<WeakPrefix>().Type);
    
public class FiercePrefix() : SimpleLeveledPrefix(
    1,
    "damage",
    PrefixCategory.AnyWeapon,
    damageMult: 1.15f, // +15% damage
    next: () => ModContent.GetInstance<BrutalPrefix>().Type,
    previous: () => ModContent.GetInstance<TemperedPrefix>().Type);
    
public class BrutalPrefix() : SimpleLeveledPrefix(
    2,
    "damage",
    PrefixCategory.AnyWeapon,
    damageMult: 1.25f, // +25% damage
    next: () => ModContent.GetInstance<SavagePrefix>().Type,
    previous: () => ModContent.GetInstance<FiercePrefix>().Type);
    
public class SavagePrefix() : SimpleLeveledPrefix(
    3,
    "damage",
    PrefixCategory.AnyWeapon,
    damageMult: 1.40f, // +40% damage
    next: () => -1,
    previous: () => ModContent.GetInstance<BrutalPrefix>().Type);