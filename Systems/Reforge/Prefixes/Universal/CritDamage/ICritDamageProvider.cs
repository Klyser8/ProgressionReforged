namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

/// <summary>
/// Interface necessary if I want a prefix to apply multiple custom stat modifiers.
/// </summary>
public interface ICritDamageProvider
{
    /// Multiplier applied *after* Terraria's normal ×2 crit
    float CritDamageMult { get; }
}