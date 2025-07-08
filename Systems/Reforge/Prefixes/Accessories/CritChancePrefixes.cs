using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class CloudedPrefixes() : SimpleAccessoryPrefixes(
    level: -1,
    chainKey: "acc.critChance",
    critBonus: -1,
    next: () => ModContent.GetInstance<AttentivePrefixes>().Type,
    previous: () => -1);

public class AttentivePrefixes() : SimpleAccessoryPrefixes(
    level: 0,
    chainKey: "acc.critChance",
    critBonus: 1,
    next: () => ModContent.GetInstance<SharpEyedPrefixes>().Type,
    previous: () => ModContent.GetInstance<CloudedPrefixes>().Type);
    
public class SharpEyedPrefixes() : SimpleAccessoryPrefixes(
    level: 1,
    chainKey: "acc.critChance",
    critBonus: 2,
    next: () => ModContent.GetInstance<InstinctivePrefixes>().Type,
    previous: () => ModContent.GetInstance<AttentivePrefixes>().Type);
    
public class InstinctivePrefixes() : SimpleAccessoryPrefixes(
    level: 2,
    chainKey: "acc.critChance",
    critBonus: 3,
    next: () => ModContent.GetInstance<LuckyPrefixes>().Type,
    previous: () => ModContent.GetInstance<SharpEyedPrefixes>().Type);

public class LuckyPrefixes() : SimpleAccessoryPrefixes(
    level: 3,
    chainKey: "acc.critChance",
    critBonus: 4,
    next: () => -1,
    previous: () => ModContent.GetInstance<InstinctivePrefixes>().Type);