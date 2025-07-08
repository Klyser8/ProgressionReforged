using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Summon;

public class UnmarkedPrefix() : SimpleWhipTagDamagePrefix(
    -1,
    "whipTagDamage",
    whipTagDamageMult: 0.90f,
    next: () => ModContent.GetInstance<FlaggedPrefix>().Type,
    previous: () => -1);

public class FlaggedPrefix() : SimpleWhipTagDamagePrefix(
    0,
    "whipTagDamage",
    whipTagDamageMult: 1.05f,
    next: () => ModContent.GetInstance<BrandingPrefix>().Type,
    previous: () => ModContent.GetInstance<UnmarkedPrefix>().Type);

public class BrandingPrefix() : SimpleWhipTagDamagePrefix(
    1,
    "whipTagDamage",
    whipTagDamageMult: 1.15f,
    next: () => ModContent.GetInstance<DesignatedPrefix>().Type,
    previous: () => ModContent.GetInstance<FlaggedPrefix>().Type);

public class DesignatedPrefix() : SimpleWhipTagDamagePrefix(
    2,
    "whipTagDamage",
    whipTagDamageMult: 1.30f,
    next: () => ModContent.GetInstance<CondemningPrefix>().Type,
    previous: () => ModContent.GetInstance<BrandingPrefix>().Type);

public class CondemningPrefix() : SimpleWhipTagDamagePrefix(
    3,
    "whipTagDamage",
    whipTagDamageMult: 1.50f,
    next: () => -1,
    previous: () => ModContent.GetInstance<DesignatedPrefix>().Type);