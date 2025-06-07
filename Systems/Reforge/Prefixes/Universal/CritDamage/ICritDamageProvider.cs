namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public interface ICritDamageProvider
{
    /// Multiplier applied *after* Terraria's normal ×2 crit
    float CritDamageMult { get; }
}