using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.AccessoryPrefixes;

public class KnockbackReductionModPlayer : ModPlayer
{
    
    public float KnockbackMult { get; set; }
    
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (KnockbackMult >= 1f)
        {
            return; // Player is immune to knockback, no need to modify
        }
        modifiers.Knockback *= KnockbackMult;
    }
}