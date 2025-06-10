using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Ranged;

public class JammedPrefix() : SimpleLeveledPrefix(
    -1,
    "useTimeForVelocity",
    PrefixCategory.Ranged,
    shootSpeedMult: 0.93f, // -7% shoot speed
    useTimeMult: 1.03f, // +3% use time
    damageMult: 0.97f, // -3% damage
    next: () => ModContent.GetInstance<DrawnPrefix>().Type,
    previous: () => -1);
    
public class DrawnPrefix() : SimpleLeveledPrefix(
    0,
    "useTimeForVelocity",
    PrefixCategory.Ranged,
    shootSpeedMult: 0.82f, // -18% shoot speed
    useTimeMult: 0.87f, // -13% use time
    damageMult: 0.93f, // -7% damage
    next: () => ModContent.GetInstance<TensedPrefix>().Type,
    previous: () => ModContent.GetInstance<JammedPrefix>().Type);
    
public class TensedPrefix() : SimpleLeveledPrefix(
    1,
    "useTimeForVelocity",
    PrefixCategory.Ranged,
    shootSpeedMult: 0.69f, // -31% shoot speed
    useTimeMult: 0.76f, // -24% use time
    damageMult: 0.88f, // -12% damage
    next: () => ModContent.GetInstance<PropelledPrefix>().Type,
    previous: () => ModContent.GetInstance<DrawnPrefix>().Type);
    
public class PropelledPrefix() : SimpleLeveledPrefix(
    2,
    "useTimeForVelocity",
    PrefixCategory.Ranged,
    shootSpeedMult: 0.53f, // -47% shoot speed
    useTimeMult: 0.64f, // -36% use time
    damageMult: 0.82f, // -18% damage
    next: () => ModContent.GetInstance<BallisticPrefix>().Type,
    previous: () => ModContent.GetInstance<TensedPrefix>().Type);
    
public class BallisticPrefix() : SimpleLeveledPrefix(
    3,
    "useTimeForVelocity",
    PrefixCategory.Ranged,
    shootSpeedMult: 0.31f, // -69% shoot speed
    useTimeMult: 0.50f, // -50% use time
    damageMult: 0.75f, // -25% damage
    next: () => -1,
    previous: () => ModContent.GetInstance<PropelledPrefix>().Type);