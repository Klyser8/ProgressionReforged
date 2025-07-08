using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class ClumsyPrefixes() : SimpleAccessoryPrefixes(
    -1,
    "acc.knockbackResist",
    knockbackResistMult: 0.90f, // -10% knockback resistance
    next: () => ModContent.GetInstance<GroundedPrefixes>().Type,
    previous: () => -1);
    
public class GroundedPrefixes() : SimpleAccessoryPrefixes(
    0,
    "acc.knockbackResist",
    knockbackResistMult: 1.05f, // +5% knockback resistance
    next: () => ModContent.GetInstance<BracedPrefixes>().Type,
    previous: () => ModContent.GetInstance<ClumsyPrefixes>().Type);
    
public class BracedPrefixes() : SimpleAccessoryPrefixes(
    1,
    "acc.knockbackResist",
    knockbackResistMult: 1.10f, // +10% knockback resistance
    next: () => ModContent.GetInstance<SteadfastPrefixes>().Type,
    previous: () => ModContent.GetInstance<GroundedPrefixes>().Type);
    
public class SteadfastPrefixes() : SimpleAccessoryPrefixes(
    2,
    "acc.knockbackResist",
    knockbackResistMult: 1.15f, // +15% knockback resistance
    next: () => ModContent.GetInstance<StonebornPrefixes>().Type,
    previous: () => ModContent.GetInstance<BracedPrefixes>().Type);
    
public class StonebornPrefixes() : SimpleAccessoryPrefixes(
    3,
    "acc.knockbackResist",
    knockbackResistMult: 1.20f, // +20% knockback resistance
    next: () => -1,
    previous: () => ModContent.GetInstance<SteadfastPrefixes>().Type);