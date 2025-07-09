using ProgressionReforged.Systems.Reforge.Prefixes.Universal;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public class EdgelessPrefix() : SimpleAccessoryPrefixes(
    level: -1,
    chainKey: "acc.armorPen",
    armorPenBonus: -3,
    next: () => ModContent.GetInstance<BluntPrefix>().Type,
    previous: () => -1);
    
public class BluntPrefix() : SimpleAccessoryPrefixes(
    level: 0,
    chainKey: "acc.armorPen",
    armorPenBonus: -2,
    next: () => ModContent.GetInstance<SafePrefix>().Type,
    previous: () => ModContent.GetInstance<EdgelessPrefix>().Type);
    
public class SafePrefix() : SimpleAccessoryPrefixes(
    level: 1,
    chainKey: "acc.armorPen",
    armorPenBonus: -1,
    next: () => ModContent.GetInstance<PiercingPrefix>().Type,
    previous: () => ModContent.GetInstance<BluntPrefix>().Type);

//TODO: 1. Fix armor pen pricing 2. Upgrade UI bugs out with accessories

public class PiercingPrefix() : SimpleAccessoryPrefixes(
    level: 2,
    chainKey: "acc.armorPen",
    armorPenBonus: 1,
    next: () => ModContent.GetInstance<InvasivePrefix>().Type,
    previous: () => ModContent.GetInstance<SafePrefix>().Type);
    
public class InvasivePrefix() : SimpleAccessoryPrefixes(
    level: 3,
    chainKey: "acc.armorPen",
    armorPenBonus: 2,
    next: () => -1,
    previous: () => ModContent.GetInstance<PiercingPrefix>().Type);