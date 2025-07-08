using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class FragilePrefixes() : SimpleAccessoryPrefixes(
    level: -1,
    chainKey: "acc.defense",
    defenseBonus: -1,
    next: () => ModContent.GetInstance<SturdyPrefixes>().Type,
    previous: () => -1);

public class SturdyPrefixes() : SimpleAccessoryPrefixes(
    level: 0,
    chainKey: "acc.defense",
    defenseBonus: 1,
    next: () => ModContent.GetInstance<RobustPrefixes>().Type,
    previous: () => ModContent.GetInstance<FragilePrefixes>().Type);

public class RobustPrefixes() : SimpleAccessoryPrefixes(
    level: 1,
    chainKey: "acc.defense",
    defenseBonus: 2,
    next: () => ModContent.GetInstance<FortifiedPrefixes>().Type,
    previous: () => ModContent.GetInstance<SturdyPrefixes>().Type);

public class FortifiedPrefixes() : SimpleAccessoryPrefixes(
    level: 2,
    chainKey: "acc.defense",
    defenseBonus: 3,
    next: () => ModContent.GetInstance<WardingPrefixes>().Type,
    previous: () => ModContent.GetInstance<RobustPrefixes>().Type);

public class WardingPrefixes() : SimpleAccessoryPrefixes(
    level: 3,
    chainKey: "acc.defense",
    defenseBonus: 4,
    next: () => -1,
    previous: () => ModContent.GetInstance<FortifiedPrefixes>().Type);
