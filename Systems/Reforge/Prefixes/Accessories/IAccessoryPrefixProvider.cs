namespace ProgressionReforged.Systems.Reforge.Prefixes.Accessories;

public interface IAccessoryPrefixProvider
{
    int DefenseBonus { get; }
    int HealthBonus { get; }
    int CritBonus { get; }
    int ArmorPenBonus { get; }
    float CritDamageMult { get; }
    float JumpHeightMult { get; }
    float KnockbackMult { get; }
    float DamageMult { get; }
    float ManaRegenMult { get; }
    float MovementSpeedMult { get; }
}