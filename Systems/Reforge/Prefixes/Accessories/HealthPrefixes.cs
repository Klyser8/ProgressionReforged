using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class EmptyPrefixes() : SimpleAccessoryPrefixes(
    level: -1,
    chainKey: "acc.health",
    healthBonus: -5,
    next: () => ModContent.GetInstance<RootedPrefixes>().Type,
    previous: () => -1);

public class RootedPrefixes() : SimpleAccessoryPrefixes(
    level: 0,
    chainKey: "acc.health",
    healthBonus: 5,
    next: () => ModContent.GetInstance<WholesomePrefixes>().Type,
    previous: () => ModContent.GetInstance<EmptyPrefixes>().Type);

public class WholesomePrefixes() : SimpleAccessoryPrefixes(
    level: 1,
    chainKey: "acc.health",
    healthBonus: 10,
    next: () => ModContent.GetInstance<VigorousPrefixes>().Type,
    previous: () => ModContent.GetInstance<RootedPrefixes>().Type);

public class VigorousPrefixes() : SimpleAccessoryPrefixes(
    level: 2,
    chainKey: "acc.health",
    healthBonus: 15,
    next: () => ModContent.GetInstance<VitalPrefixes>().Type,
    previous: () => ModContent.GetInstance<WholesomePrefixes>().Type);

public class VitalPrefixes() : SimpleAccessoryPrefixes(
    level: 3,
    chainKey: "acc.health",
    healthBonus: 20,
    next: () => -1,
    previous: () => ModContent.GetInstance<VigorousPrefixes>().Type);
