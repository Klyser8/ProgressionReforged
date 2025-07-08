using ProgressionReforged.Systems.Reforge.Prefixes.Magic;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.SimplePrefixes;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee;

public class UnwieldyPrefix() : SimpleSizePrefix(
    -1,
    "damageForUseTime",
    PrefixCategory.Melee,
    damageMult: 1.05f, // +4% damage
    scaleMult: 1.01f, // +1% scale
    useTimeMult: 1.07f, // +7% use time
    next: () => ModContent.GetInstance<DensePrefix>().Type,
    previous: () => -1);
    
public class DensePrefix() : SimpleSizePrefix(
    0,
    "damageForUseTime",
    PrefixCategory.Melee,
    damageMult: 1.17f, // +17% damage
    scaleMult: 1.03f, // +3% scale
    useTimeMult: 1.17f, // +17% use time
    next: () => ModContent.GetInstance<HeftyPrefix>().Type,
    previous: () => ModContent.GetInstance<UnwieldyPrefix>().Type);
    
public class HeftyPrefix() : SimpleSizePrefix(
    1,
    "damageForUseTime",
    PrefixCategory.Melee,
    damageMult: 1.30f, // +30% damage
    scaleMult: 1.09f, // +9% scale
    useTimeMult: 1.30f, // +30% use time
    next: () => ModContent.GetInstance<CrushingPrefix>().Type,
    previous: () => ModContent.GetInstance<DensePrefix>().Type);

public class CrushingPrefix() : SimpleSizePrefix(
    2,
    "damageForUseTime",
    PrefixCategory.Melee,
    damageMult: 1.48f, // +48% damage
    scaleMult: 1.18f, // +18% scale
    useTimeMult: 1.48f, // +48% use time
    next: () => ModContent.GetInstance<EarthshakingPrefix>().Type,
    previous: () => ModContent.GetInstance<HeftyPrefix>().Type);

public class EarthshakingPrefix() : SimpleSizePrefix(
    3,
    "damageForUseTime",
    PrefixCategory.Melee,
    damageMult: 1.70f, // +70% damage
    scaleMult: 1.30f, // +30% scale
    useTimeMult: 1.70f, // +70% use time
    next: () => -1,
    previous: () => ModContent.GetInstance<CrushingPrefix>().Type);
    
