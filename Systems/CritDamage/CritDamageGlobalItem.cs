using System;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.CritDamage;

public class CritDamageGlobalItem : GlobalItem
{

    public override bool InstancePerEntity => true;

    public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
    {
        float mult = player.GetModPlayer<AccessoryPrefixes.AccessoryCritDamagePlayer>().CritDamageMult;
        if (item.prefix > 0 &&
            PrefixLoader.GetPrefix(item.prefix) is ICritDamageProvider p)
        {
            mult *= p.CritDamageMult;
        }
        if (Math.Abs(mult - 1f) > 0.001f)
            modifiers.CritDamage *= mult;
    }
}