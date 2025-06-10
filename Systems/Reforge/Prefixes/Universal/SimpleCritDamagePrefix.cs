using System;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal;

public abstract class SimpleCritDamagePrefix(int level,
        string chainKey,
        PrefixCategory category,
        float critDamageMult = 1.00f,
        float damageMult = 1.00f,
        float knockbackMult = 1.00f,
        float useTimeMult = 1.00f,
        float scaleMult = 1.00f,
        float shootSpeedMult = 1.00f,
        float manaMult = 1.00f,
        int critBonus = 0,
        Func<int>? next = null,
        Func<int>? previous = null)
    : CritDamagePrefix(level, chainKey)
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

    public override PrefixCategory Category => category;
    public override float CritDamageMult => critDamageMult;
    public override int GetNext() => _next();
    public override int GetPrevious() => _previous();
}