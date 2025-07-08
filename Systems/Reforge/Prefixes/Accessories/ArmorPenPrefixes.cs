using ProgressionReforged.Systems.Reforge.Prefixes.Universal;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class BluntPrefixes() : SimpleAccessoryPrefixes(
    level: -1,
    chainKey: "acc.armorPen",
    armorPenBonus: -3,
    next: () => ModContent.GetInstance<EdgyPrefixes>().Type,
    previous: () => -1);
    
public class EdgyPrefixes() : SimpleAccessoryPrefixes(
    level: 0,
    chainKey: "acc.armorPen",
    armorPenBonus: -2,
    next: () => ModContent.GetInstance<PiercingPrefixes>().Type,
    previous: () => ModContent.GetInstance<BluntPrefixes>().Type);
    
public class PiercingPrefixes() : SimpleAccessoryPrefixes(
    level: 1,
    chainKey: "acc.armorPen",
    armorPenBonus: -1,
    next: () => ModContent.GetInstance<BreachingPrefixes>().Type,
    previous: () => ModContent.GetInstance<EdgyPrefixes>().Type);

public class BreachingPrefixes() : SimpleAccessoryPrefixes(
    level: 2,
    chainKey: "acc.armorPen",
    armorPenBonus: 1,
    next: () => ModContent.GetInstance<InvasivePrefixes>().Type,
    previous: () => ModContent.GetInstance<PiercingPrefixes>().Type);
    
public class InvasivePrefixes() : SimpleAccessoryPrefixes(
    level: 3,
    chainKey: "acc.armorPen",
    armorPenBonus: 2,
    next: () => -1,
    previous: () => ModContent.GetInstance<BreachingPrefixes>().Type);