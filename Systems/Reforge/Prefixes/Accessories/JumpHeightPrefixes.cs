using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class WeightedPrefixes() : SimpleAccessoryPrefixes(
    -1,
    "acc.jumpHeight",
    jumpHeightMult: 0.88f, // -12% jump height
    next: () => ModContent.GetInstance<BuoyantPrefixes>().Type,
    previous: () => -1);
    
public class BuoyantPrefixes() : SimpleAccessoryPrefixes(
    0,
    "acc.jumpHeight",
    jumpHeightMult: 1.06f, // +6% jump height
    next: () => ModContent.GetInstance<SpringyPrefixes>().Type,
    previous: () => ModContent.GetInstance<WeightedPrefixes>().Type);
    
public class SpringyPrefixes() : SimpleAccessoryPrefixes(
    1,
    "acc.jumpHeight",
    jumpHeightMult: 1.12f, // +12% jump height
    next: () => ModContent.GetInstance<LeapingPrefixes>().Type,
    previous: () => ModContent.GetInstance<BuoyantPrefixes>().Type);
    
public class LeapingPrefixes() : SimpleAccessoryPrefixes(
    2,
    "acc.jumpHeight",
    jumpHeightMult: 1.18f, // +18% jump height
    next: () => ModContent.GetInstance<AirbornPrefixes>().Type,
    previous: () => ModContent.GetInstance<SpringyPrefixes>().Type);
    
public class AirbornPrefixes() : SimpleAccessoryPrefixes(
    3,
    "acc.jumpHeight",
    jumpHeightMult: 1.24f, // +24% jump height
    next: () => -1,
    previous: () => ModContent.GetInstance<LeapingPrefixes>().Type);