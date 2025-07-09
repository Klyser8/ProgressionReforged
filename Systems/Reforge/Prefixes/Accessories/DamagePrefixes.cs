using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

// All damage multipliers need to be offset by 0.01 in either direction, due to some strange rounding errors.
public class PowerlessPrefix() : SimpleAccessoryPrefixes(
    -1,
    "acc.damage",
    damageMult: 0.97f, // -2% damage
    next: () => ModContent.GetInstance<DrivenPrefix>().Type,
    previous: () => -1);
    
public class DrivenPrefix() : SimpleAccessoryPrefixes(
    0,
    "acc.damage",
    damageMult: 1.02f, // +1% damage
    next: () => ModContent.GetInstance<EmpoweredPrefix>().Type,
    previous: () => ModContent.GetInstance<PowerlessPrefix>().Type);

public class EmpoweredPrefix() : SimpleAccessoryPrefixes(
    1,
    "acc.damage",
    damageMult: 1.03f, // +2% damage
    next: () => ModContent.GetInstance<RelentlessPrefix>().Type,
    previous: () => ModContent.GetInstance<DrivenPrefix>().Type);
    
public class RelentlessPrefix() : SimpleAccessoryPrefixes(
    2,
    "acc.damage",
    damageMult: 1.04f, // +3% damage
    next: () => ModContent.GetInstance<MenacingPrefix>().Type,
    previous: () => ModContent.GetInstance<EmpoweredPrefix>().Type);
    
public class MenacingPrefix() : SimpleAccessoryPrefixes(
    3,
    "acc.damage",
    damageMult: 1.05f, // +4% damage
    next: () => -1,
    previous: () => ModContent.GetInstance<RelentlessPrefix>().Type);