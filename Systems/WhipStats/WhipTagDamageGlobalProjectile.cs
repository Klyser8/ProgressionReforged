using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ProgressionReforged.Content.Buffs;
using ProgressionReforged.Systems.Reforge.Prefixes.Summon;
using Terraria.ID;

namespace ProgressionReforged.Systems.WhipStats;

public class WhipTagDamageGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    private float _tagDamageMult = 1f;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (!projectile.friendly)
            return;

        if (source is EntitySource_ItemUse itemSource)
        {
            if (itemSource.Item.prefix > 0 && PrefixLoader.GetPrefix(itemSource.Item.prefix) is IWhipTagDamageProvider p)
            {
                _tagDamageMult = p.WhipTagDamageMult;
            }
        }
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (_tagDamageMult == 1f || !ProjectileID.Sets.IsAWhip[projectile.type])
            return;

        target.AddBuff(ModContent.BuffType<PrefixWhipTagDamageBuff>(), 240);
        target.GetGlobalNPC<WhipTagDamageGlobalNPC>().ApplyMultiplier(_tagDamageMult);
    }
}