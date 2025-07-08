using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic;

public class VolatilePrefix() : SimpleLeveledPrefix(
    -1,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 1.20f, // +20% mana cost
    damageMult: 1.02f, // +2% damage
    useTimeMult: 0.99f, // -1% use time
    next: () => ModContent.GetInstance<SparkingPrefix>().Type,
    previous: () => -1);

public class SparkingPrefix() : SimpleLeveledPrefix(
    0,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 1.50f, // +50% mana cost
    damageMult: 1.08f, // +8% damage
    useTimeMult: 0.94f, // -6% use time
    next: () => ModContent.GetInstance<BurstingPrefix>().Type,
    previous: () => ModContent.GetInstance<VolatilePrefix>().Type);
    
public class BurstingPrefix() : SimpleLeveledPrefix(
    1,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 2.10f, // +110% mana cost
    damageMult: 1.19f, // +19% damage
    useTimeMult: 0.87f, // -13% use time
    next: () => ModContent.GetInstance<SurgingPrefix>().Type,
    previous: () => ModContent.GetInstance<SparkingPrefix>().Type);
    
public class SurgingPrefix() : SimpleLeveledPrefix(
    2,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 2.90f, // +190% mana cost
    damageMult: 1.32f, // +32% damage
    useTimeMult: 0.79f, // -21% use time
    next: () => ModContent.GetInstance<UnboundPrefix>().Type,
    previous: () => ModContent.GetInstance<BurstingPrefix>().Type);
    
public class UnboundPrefix() : SimpleLeveledPrefix(
    3,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 4.00f, // +300% mana cost
    damageMult: 1.47f, // +47% damage
    useTimeMult: 0.70f, // -30% use time
    next: () => -1,
    previous: () => ModContent.GetInstance<SurgingPrefix>().Type);