using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.AccessoryPrefixes;

public class AccessoryCritDamagePlayer : ModPlayer
{
    public float CritDamageMult { get; set; } = 1f;

    public override void ResetEffects()
    {
        CritDamageMult = 1f;
    }
}