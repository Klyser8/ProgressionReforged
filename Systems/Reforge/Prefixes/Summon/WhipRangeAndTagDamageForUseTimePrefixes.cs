using ProgressionReforged.Systems.Reforge.Prefixes.Universal.SimplePrefixes;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Summon;

public class LimpPrefix() : SimpleWhipTagDamagePrefix(
    -1,
    "rangeAndTagDamageForUseTime",
    whipTagDamageMult: 1.10f, // +10% tag damage
    damageMult: 1.03f, // +3% damage
    useTimeMult: 1.10f, // +10% use time
    next: () => ModContent.GetInstance<MeasuredPrefix>().Type,
    previous: () => -1);
    
public class MeasuredPrefix() : SimpleWhipTagDamagePrefix(
    0,
    "rangeAndTagDamageForUseTime",
    whipTagDamageMult: 1.30f, // +30% tag damage
    damageMult: 1.08f, // +8% damage
    useTimeMult: 1.25f, // +25% use time
    next: () => ModContent.GetInstance<TensionedPrefix>().Type,
    previous: () => ModContent.GetInstance<LimpPrefix>().Type);
    
public class TensionedPrefix() : SimpleWhipTagDamagePrefix(
    1,
    "rangeAndTagDamageForUseTime",
    whipTagDamageMult: 1.66f, // +66% tag damage
    damageMult: 1.15f, // +15% damage
    useTimeMult: 1.45f, // +45% use time
    next: () => ModContent.GetInstance<ViperousPrefix>().Type,
    previous: () => ModContent.GetInstance<MeasuredPrefix>().Type);
    
public class ViperousPrefix() : SimpleWhipTagDamagePrefix(
    2,
    "rangeAndTagDamageForUseTime",
    whipTagDamageMult: 2.11f, // +111% tag damage
    damageMult: 1.24f, // +24% damage
    useTimeMult: 1.70f, // +70% use time
    next: () => ModContent.GetInstance<DomineeringPrefix>().Type,
    previous: () => ModContent.GetInstance<TensionedPrefix>().Type);
    
public class DomineeringPrefix() : SimpleWhipTagDamagePrefix(
    3,
    "rangeAndTagDamageForUseTime",
    whipTagDamageMult: 2.69f, // +169% tag damage
    damageMult: 1.38f, // +38% damage
    useTimeMult: 2.00f, // +100% use time
    next: () => -1,
    previous: () => ModContent.GetInstance<ViperousPrefix>().Type);