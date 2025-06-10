using System;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes;

public abstract class SimpleLeveledPrefix(int level,
        string chainKey,
        PrefixCategory category,
        float damageMult = 1f,
        float knockbackMult = 1f,
        float useTimeMult = 1f,
        float scaleMult = 1f,
        float shootSpeedMult = 1f,
        float manaMult = 1f,
        int critBonus = 0,
        Func<int>? next = null,
        Func<int>? previous = null)
    : LeveledPrefix(level, chainKey)
{
    private readonly Func<int> _next = next ?? (() => -1);
    private readonly Func<int> _previous = previous ?? (() => -1);

    public override PrefixCategory Category => category;

    public override void SetStats(
        ref float _damageMult,
        ref float _knockbackMult,
        ref float _useTimeMult,
        ref float _scaleMult,
        ref float _shootSpeedMult,
        ref float _manaMult,
        ref int _critBonus)
    {
        _damageMult = damageMult;
        _knockbackMult = knockbackMult;
        _useTimeMult = useTimeMult;
        _scaleMult = scaleMult;
        _shootSpeedMult = shootSpeedMult;
        _manaMult = manaMult;
        _critBonus = critBonus;
        base.SetStats(ref _damageMult, ref _knockbackMult, ref _useTimeMult,
            ref _scaleMult, ref _shootSpeedMult, ref _manaMult, ref _critBonus);
    }

    public override int GetNext() => _next();
    public override int GetPrevious() => _previous();
}