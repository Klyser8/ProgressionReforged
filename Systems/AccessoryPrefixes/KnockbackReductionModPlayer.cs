using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.AccessoryPrefixes;

public class KnockbackReductionModPlayer : ModPlayer
{
    public float KnockbackMult { get; set; } = 1f;

    public override void ResetEffects()
    {
        KnockbackMult = 1f;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (KnockbackMult <= 0f)
        {
            modifiers.Knockback.Flat = 0f;
            return;
        }

        modifiers.Knockback *= KnockbackMult;
    }
}