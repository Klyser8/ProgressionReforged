using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged;

public class HamperedPrefix() : SimpleLeveledPrefix(
    -1,
    "shootSpeed",
    PrefixCategory.Ranged,
    shootSpeedMult: 0.85f, // -15% velocity
    next: () => ModContent.GetInstance<RefinedPrefix>().Type,
    previous: () => -1);

public class RefinedPrefix() : SimpleLeveledPrefix(
    0,
    "shootSpeed",
    PrefixCategory.Ranged,
    shootSpeedMult: 1.10f, // +10% velocity
    next: () => ModContent.GetInstance<AcceleratedPrefix>().Type,
    previous: () => ModContent.GetInstance<HamperedPrefix>().Type);
    
public class AcceleratedPrefix() : SimpleLeveledPrefix(
    1,
    "shootSpeed",
    PrefixCategory.Ranged,
    shootSpeedMult: 1.30f, // +30% velocity
    next: () => ModContent.GetInstance<SonicPrefix>().Type,
    previous: () => ModContent.GetInstance<RefinedPrefix>().Type);
    
public class SonicPrefix() : SimpleLeveledPrefix(
    2,
    "shootSpeed",
    PrefixCategory.Ranged,
    shootSpeedMult: 1.60f, // +60% velocity
    next: () => ModContent.GetInstance<BlazingPrefix>().Type,
    previous: () => ModContent.GetInstance<AcceleratedPrefix>().Type);

public class BlazingPrefix() : SimpleLeveledPrefix(
    3,
    "shootSpeed",
    PrefixCategory.Ranged,
    shootSpeedMult: 2.00f, // +100% velocity
    next: () => -1, // No next prefix
    previous: () => ModContent.GetInstance<SonicPrefix>().Type);