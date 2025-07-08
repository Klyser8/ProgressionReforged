using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class DrainedPrefix() : SimpleAccessoryPrefixes(
    -1,
    "acc.manaRegen",
    manaRegenMult: 0.90f, // -10% mana regen
    next: () => ModContent.GetInstance<BalancedPrefix>().Type,
    previous: () => -1);
    
public class BalancedPrefix() : SimpleAccessoryPrefixes(
    0,
    "acc.manaRegen",
    manaRegenMult: 1.10f, // +10% mana regen
    next: () => ModContent.GetInstance<InfusedPrefix>().Type,
    previous: () => ModContent.GetInstance<DrainedPrefix>().Type);
    
public class InfusedPrefix() : SimpleAccessoryPrefixes(
    1,
    "acc.manaRegen",
    manaRegenMult: 1.20f, // +20% mana regen
    next: () => ModContent.GetInstance<ArcanePrefix>().Type,
    previous: () => ModContent.GetInstance<BalancedPrefix>().Type);
    
public class ArcanePrefix() : SimpleAccessoryPrefixes(
    2,
    "acc.manaRegen",
    manaRegenMult: 1.30f, // +30% mana regen
    next: () => ModContent.GetInstance<EnlightenedPrefix>().Type,
    previous: () => ModContent.GetInstance<InfusedPrefix>().Type);
    
public class EnlightenedPrefix() : SimpleAccessoryPrefixes(
    3,
    "acc.manaRegen",
    manaRegenMult: 1.40f, // +40% mana regen
    next: () => -1,
    previous: () => ModContent.GetInstance<ArcanePrefix>().Type);