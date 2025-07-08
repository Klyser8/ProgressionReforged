using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class SlowPrefix() : SimpleAccessoryPrefixes(
    -1,
    "acc.moveSpeed",
    movementSpeedMult: 0.97f, // -3% movement speed
    next: () => ModContent.GetInstance<PacingPrefix>().Type,
    previous: () => -1);

public class PacingPrefix() : SimpleAccessoryPrefixes(
    0,
    "acc.moveSpeed",
    movementSpeedMult: 1.02f, // +2% movement speed
    next: () => ModContent.GetInstance<SwiftPrefix>().Type,
    previous: () => ModContent.GetInstance<SlowPrefix>().Type);
    
public class SwiftPrefix() : SimpleAccessoryPrefixes(
    1,
    "acc.moveSpeed",
    movementSpeedMult: 1.04f, // +4% movement speed
    next: () => ModContent.GetInstance<AgilePrefix>().Type,
    previous: () => ModContent.GetInstance<PacingPrefix>().Type);
    
public class AgilePrefix() : SimpleAccessoryPrefixes(
    2,
    "acc.moveSpeed",
    movementSpeedMult: 1.06f, // +6% movement speed
    next: () => ModContent.GetInstance<QuickPrefix>().Type,
    previous: () => ModContent.GetInstance<SwiftPrefix>().Type);
    
public class QuickPrefix() : SimpleAccessoryPrefixes(
    3,
    "acc.moveSpeed",
    movementSpeedMult: 1.08f, // +8% movement speed
    next: () => -1,
    previous: () => ModContent.GetInstance<AgilePrefix>().Type);