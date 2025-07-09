using System;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.CritDamage;

public class CritDamageGlobalProjectile : GlobalProjectile
{
    
    public override bool InstancePerEntity => true;
    private float _critDamageMult = 1.00f;
    
    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (projectile.friendly && Math.Abs(_critDamageMult - 1.00f) > 0.001)
        {
            // Apply the crit damage multiplier
            modifiers.CritDamage *= _critDamageMult;
        }
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (!projectile.friendly)
        {
            return;
        }
        if (source is EntitySource_ItemUse itemSource)
        {
            // Check if the item has a crit damage prefix
            if (itemSource.Item.prefix > 0 &&
                PrefixLoader.GetPrefix(itemSource.Item.prefix) is ICritDamageProvider p)
            {
                // Apply the crit damage multiplier
                _critDamageMult *= p.CritDamageMult;
            }
            if (itemSource.Entity is Player plr)
            {
                _critDamageMult *= plr.GetModPlayer<AccessoryPrefixes.AccessoryCritDamagePlayer>().CritDamageMult;
            }
        }
    }
}