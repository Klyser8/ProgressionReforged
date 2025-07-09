using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.AccessoryPrefixes;

public class KnockbackReductionModPlayer : ModPlayer
{
    public float KnockbackReduction { get; set; }

    public override void ResetEffects()
    {
        KnockbackReduction = 0f;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        float mult = 1f - KnockbackReduction;

        if (mult <= 0f)
        {
            modifiers.Knockback.Flat = 0f;
            return;
        }

        modifiers.Knockback *= mult;
    }

    public override void PostUpdateEquips()
    {
        if (KnockbackReduction >= 1f)
            Player.noKnockback = true;
    }
}