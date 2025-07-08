using ProgressionReforged.Systems.Reforge.Prefixes.Universal.SimplePrefixes;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Melee;

public class SmallPrefix() : SimpleSizePrefix(
    -1,
    "size",
    PrefixCategory.Melee,
    scaleMult: 0.85f, // -15% size
    next: () => ModContent.GetInstance<AboveAveragePrefix>().Type,
    previous: () => -1);
    
public class AboveAveragePrefix() : SimpleSizePrefix(
    0,
    "size",
    PrefixCategory.Melee,
    scaleMult: 1.05f, // +5% size
    next: () => ModContent.GetInstance<BigPrefix>().Type,
    previous: () => ModContent.GetInstance<SmallPrefix>().Type);
    
public class BigPrefix() : SimpleSizePrefix(
    1,
    "size",
    PrefixCategory.Melee,
    scaleMult: 1.15f, // +15% size
    next: () => ModContent.GetInstance<MassivePrefix>().Type,
    previous: () => ModContent.GetInstance<AboveAveragePrefix>().Type);
    
public class MassivePrefix() : SimpleSizePrefix(
    2,
    "size",
    PrefixCategory.Melee,
    scaleMult: 1.30f, // +30% size
    next: () => ModContent.GetInstance<TitanicPrefix>().Type,
    previous: () => ModContent.GetInstance<BigPrefix>().Type);
    
public class TitanicPrefix() : SimpleSizePrefix(
    3,
    "size",
    PrefixCategory.Melee,
    scaleMult: 1.60f, // +60% size
    next: () => -1,
    previous: () => ModContent.GetInstance<MassivePrefix>().Type);