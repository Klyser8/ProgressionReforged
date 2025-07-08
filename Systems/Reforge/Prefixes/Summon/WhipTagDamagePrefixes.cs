using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Summon;

public class UnmarkedPrefix() : SimpleWhipTagDamagePrefix(
    -1,
    "whipTagDamage",
    whipTagDamageMult: 0.75f, // -25% whip tag damage
    next: () => ModContent.GetInstance<FlaggedPrefix>().Type,
    previous: () => -1);

public class FlaggedPrefix() : SimpleWhipTagDamagePrefix(
    0,
    "whipTagDamage",
    whipTagDamageMult: 1.10f, // +10% whip tag damage
    next: () => ModContent.GetInstance<BrandingPrefix>().Type,
    previous: () => ModContent.GetInstance<UnmarkedPrefix>().Type);

public class BrandingPrefix() : SimpleWhipTagDamagePrefix(
    1,
    "whipTagDamage",
    whipTagDamageMult: 1.25f, // +25% whip tag damage
    next: () => ModContent.GetInstance<DesignatedPrefix>().Type,
    previous: () => ModContent.GetInstance<FlaggedPrefix>().Type);

public class DesignatedPrefix() : SimpleWhipTagDamagePrefix(
    2,
    "whipTagDamage",
    whipTagDamageMult: 1.45f, // +45% whip tag damage
    next: () => ModContent.GetInstance<CondemningPrefix>().Type,
    previous: () => ModContent.GetInstance<BrandingPrefix>().Type);

public class CondemningPrefix() : SimpleWhipTagDamagePrefix(
    3,
    "whipTagDamage",
    whipTagDamageMult: 1.70f, // +70% whip tag damage
    next: () => -1,
    previous: () => ModContent.GetInstance<DesignatedPrefix>().Type);