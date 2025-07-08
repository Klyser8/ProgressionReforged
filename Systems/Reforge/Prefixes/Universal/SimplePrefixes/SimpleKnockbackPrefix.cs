using System;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

// A SimpleLeveledPrefix implementation for knockback prefixes, to prevent them from appearing on items that don't have knockback.
public class SimpleKnockbackPrefix(
    int level, 
    string chainKey, 
    PrefixCategory category, 
    float damageMult = 1, 
    float knockbackMult = 1, 
    float useTimeMult = 1, 
    float scaleMult = 1, 
    float shootSpeedMult = 1, 
    float manaMult = 1, 
    int critBonus = 0, 
    Func<int> next = null, 
    Func<int> previous = null) : SimpleLeveledPrefix(level, chainKey, category, damageMult, knockbackMult, useTimeMult, scaleMult, shootSpeedMult, manaMult, critBonus, next, previous)
{
    public override bool CanRoll(Item item)
    {
        return item.knockBack > 0f && base.CanRoll(item);
    }
}