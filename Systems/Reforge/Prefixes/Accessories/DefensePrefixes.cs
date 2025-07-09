using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class FragilePrefix() : SimpleAccessoryPrefixes(
    level: -1,
    chainKey: "acc.defense",
    defenseBonus: -1,
    next: () => ModContent.GetInstance<SturdyPrefix>().Type,
    previous: () => -1);

public class SturdyPrefix() : SimpleAccessoryPrefixes(
    level: 0,
    chainKey: "acc.defense",
    defenseBonus: 1,
    next: () => ModContent.GetInstance<RobustPrefix>().Type,
    previous: () => ModContent.GetInstance<FragilePrefix>().Type);

public class RobustPrefix() : SimpleAccessoryPrefixes(
    level: 1,
    chainKey: "acc.defense",
    defenseBonus: 2,
    next: () => ModContent.GetInstance<FortifiedPrefix>().Type,
    previous: () => ModContent.GetInstance<SturdyPrefix>().Type);

public class FortifiedPrefix() : SimpleAccessoryPrefixes(
    level: 2,
    chainKey: "acc.defense",
    defenseBonus: 3,
    next: () => ModContent.GetInstance<WardingPrefix>().Type,
    previous: () => ModContent.GetInstance<RobustPrefix>().Type);

public class WardingPrefix() : SimpleAccessoryPrefixes(
    level: 3,
    chainKey: "acc.defense",
    defenseBonus: 4,
    next: () => -1,
    previous: () => ModContent.GetInstance<FortifiedPrefix>().Type);
