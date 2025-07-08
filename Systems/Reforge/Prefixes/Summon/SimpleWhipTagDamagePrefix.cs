using System;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Summon;

public abstract class SimpleWhipTagDamagePrefix(int level,
        string chainKey,
        float whipTagDamageMult = 1.00f,
        float damageMult = 1.00f,
        float knockbackMult = 1.00f,
        float useTimeMult = 1.00f,
        float scaleMult = 1.00f,
        float shootSpeedMult = 1.00f,
        float manaMult = 1.00f,
        int critBonus = 0,
        Func<int>? next = null,
        Func<int>? previous = null)
    : WhipTagDamagePrefix(level, chainKey)
{
    public override void SetStats(ref float _damageMult, ref float _knockbackMult, ref float _useTimeMult, ref float _scaleMult,
        ref float _shootSpeedMult, ref float _manaMult, ref int _critBonus)
    {
        _damageMult = damageMult;
        _knockbackMult = knockbackMult;
        _useTimeMult = useTimeMult;
        _scaleMult = scaleMult;
        _shootSpeedMult = shootSpeedMult;
        _manaMult = manaMult;
        _critBonus = critBonus;
        base.SetStats(ref _damageMult, ref _knockbackMult, ref _useTimeMult, ref _scaleMult, ref _shootSpeedMult, ref _manaMult, ref _critBonus);
    }

    private readonly Func<int> _next = next ?? (() => -1);
    private readonly Func<int> _previous = previous ?? (() => -1);

    public override PrefixCategory Category => PrefixCategory.Melee;
    
    public override bool CanRoll(Item item)
    {
        if (item.DamageType == DamageClass.SummonMeleeSpeed)
        {
            return RollChance(item) > 0;
        }

        return false;
    }
    public override float WhipTagDamageMult => whipTagDamageMult;
    public override int GetNext() => _next();
    public override int GetPrevious() => _previous();
}