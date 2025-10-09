using ProgressionReforged.Systems.Reforge.Prefixes.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.AccessoryPrefixes;

public class AccessoryPrefixGlobalItem : GlobalItem
{
    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is IAccessoryPrefixProvider p)
        {
            player.statDefense += p.DefenseBonus;
            player.statLifeMax2 += p.HealthBonus;
            player.GetCritChance(DamageClass.Generic) += p.CritBonus;
            player.GetArmorPenetration(DamageClass.Generic) += p.ArmorPenBonus;
            player.GetDamage(DamageClass.Generic) += p.DamageMult - 1f;
            player.manaRegenBonus += (int)((p.ManaRegenMult - 1f) * 100f);
            player.moveSpeed *= p.MovementSpeedMult;
            player.accRunSpeed *= p.MovementSpeedMult;
            float baseJumpSpeed = Player.jumpSpeed + player.jumpSpeedBoost;
            player.jumpSpeedBoost += baseJumpSpeed * (p.JumpHeightMult - 1f);
            // The float kept in KnockbackReductionModPlayer needs to be modified accordingly
            if (player.TryGetModPlayer(out KnockbackReductionModPlayer knockbackPlayer))
            {
                knockbackPlayer.KnockbackReduction += 1f - p.KnockbackMult;
            }
            if (player.TryGetModPlayer(out AccessoryCritDamagePlayer critPlayer))
            {
                critPlayer.CritDamageMult *= p.CritDamageMult;
            }
        }
    }
}