using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class SluggishPrefix() : SimpleLeveledPrefix(
    -1,
    "useTime",
    PrefixCategory.AnyWeapon,
    useTimeMult: 1.15f, // +15% use time
    next: () => ModContent.GetInstance<SnappyPrefix>().Type,
    previous: () => -1);
    
public class SnappyPrefix() : SimpleLeveledPrefix(
    0,
    "useTime",
    PrefixCategory.AnyWeapon,
    useTimeMult: 0.95f, // -5% use time
    next: () => ModContent.GetInstance<BriskPrefix>().Type,
    previous: () => ModContent.GetInstance<SluggishPrefix>().Type);
    
public class BriskPrefix() : SimpleLeveledPrefix(
    1,
    "useTime",
    PrefixCategory.AnyWeapon,
    useTimeMult: 0.89f, // -11% use time
    next: () => ModContent.GetInstance<NimblePrefix>().Type,
    previous: () => ModContent.GetInstance<SnappyPrefix>().Type);
    
public class NimblePrefix() : SimpleLeveledPrefix(
    2,
    "useTime",
    PrefixCategory.AnyWeapon,
    useTimeMult: 0.82f, // -18% use time
    next: () => ModContent.GetInstance<RecklessPrefix>().Type,
    previous: () => ModContent.GetInstance<BriskPrefix>().Type);
    
public class RecklessPrefix() : SimpleLeveledPrefix(
    3,
    "useTime",
    PrefixCategory.AnyWeapon,
    useTimeMult: 0.75f, // -25% use time
    next: () => -1,
    previous: () => ModContent.GetInstance<NimblePrefix>().Type);