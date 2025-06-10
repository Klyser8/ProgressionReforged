using ProgressionReforged.Systems.Reforge.Prefixes.Universal;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Magic;

public class InefficientPrefix() : SimpleLeveledPrefix(
    -1,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 1.13f, // +13% mana cost
    next: () => ModContent.GetInstance<ControlledPrefix>().Type,
    previous: () => -1);
    
public class ControlledPrefix() : SimpleLeveledPrefix(
    0,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 0.89f, // -11% mana cost
    next: () => ModContent.GetInstance<AttunedPrefix>().Type,
    previous: () => ModContent.GetInstance<InefficientPrefix>().Type);
    
public class AttunedPrefix() : SimpleLeveledPrefix(
    1,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 0.78f, // -22% mana cost
    next: () => ModContent.GetInstance<FocusedPrefix>().Type,
    previous: () => ModContent.GetInstance<ControlledPrefix>().Type);
    
public class FocusedPrefix() : SimpleLeveledPrefix(
    2,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 0.67f, // -33% mana cost
    next: () => ModContent.GetInstance<MysticPrefix>().Type,
    previous: () => ModContent.GetInstance<AttunedPrefix>().Type);
    
public class MysticPrefix() : SimpleLeveledPrefix(
    3,
    "manaCost",
    PrefixCategory.Magic,
    manaMult: 0.50f, // -50% mana cost
    next: () => -1,
    previous: () => ModContent.GetInstance<FocusedPrefix>().Type);