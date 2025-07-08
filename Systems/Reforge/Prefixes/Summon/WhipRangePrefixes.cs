using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Summon;

public class StubbyPrefix() : SimpleWhipRangePrefix(
    -1,
    "whipRange",
    whipRangeMult: 0.90f, // -10% whip range
    next: () => ModContent.GetInstance<LengthenedPrefix>().Type,
    previous: () => -1);
    
public class LengthenedPrefix() : SimpleWhipRangePrefix(
    0,
    "whipRange",
    whipRangeMult: 1.05f, // +5% whip range
    next: () => ModContent.GetInstance<ExtendedPrefix>().Type,
    previous: () => ModContent.GetInstance<StubbyPrefix>().Type);
    
public class ExtendedPrefix() : SimpleWhipRangePrefix(
    1,
    "whipRange",
    whipRangeMult: 1.15f, // +15% whip range
    next: () => ModContent.GetInstance<SpanningPrefix>().Type,
    previous: () => ModContent.GetInstance<LengthenedPrefix>().Type);
    
public class SpanningPrefix() : SimpleWhipRangePrefix(
    2,
    "whipRange",
    whipRangeMult: 1.30f, // +30% whip range
    next: () => ModContent.GetInstance<InfinitePrefix>().Type,
    previous: () => ModContent.GetInstance<ExtendedPrefix>().Type);
    
public class InfinitePrefix() : SimpleWhipRangePrefix(
    3,
    "whipRange",
    whipRangeMult: 1.50f, // +50% whip range
    next: () => -1,
    previous: () => ModContent.GetInstance<SpanningPrefix>().Type);