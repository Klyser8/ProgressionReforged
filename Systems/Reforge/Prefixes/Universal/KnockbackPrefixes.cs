﻿using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public class FeeblePrefix() : SimpleKnockbackPrefix(
    -1,
    "knockback",
    PrefixCategory.AnyWeapon,
    knockbackMult: 0.80f, // -20% knockback
    next: () => ModContent.GetInstance<RudePrefix>().Type,
    previous: () => -1);

public class RudePrefix() : SimpleKnockbackPrefix(
    0,
    "knockback",
    PrefixCategory.AnyWeapon,
    knockbackMult: 1.15f, // +15% knockback
    next: () => ModContent.GetInstance<ForcefulPrefix>().Type,
    previous: () => ModContent.GetInstance<FeeblePrefix>().Type);

public class ForcefulPrefix() : SimpleKnockbackPrefix(
    1,
    "knockback",
    PrefixCategory.AnyWeapon,
    knockbackMult: 1.30f, // +20% knockback
    next: () => ModContent.GetInstance<PowerfulPrefix>().Type,
    previous: () => ModContent.GetInstance<RudePrefix>().Type);

public class PowerfulPrefix() : SimpleKnockbackPrefix(
    2,
    "knockback",
    PrefixCategory.AnyWeapon,
    knockbackMult: 1.45f, // +45% knockback
    next: () => ModContent.GetInstance<OverwhelmingPrefix>().Type,
    previous: () => ModContent.GetInstance<ForcefulPrefix>().Type);

public class OverwhelmingPrefix() : SimpleKnockbackPrefix(
    3,
    "knockback",
    PrefixCategory.AnyWeapon,
    knockbackMult: 1.66f, // +66% knockback,
    next: () => -1,
    previous: () => ModContent.GetInstance<PowerfulPrefix>().Type);