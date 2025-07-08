using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ProgressionReforged.Content.Buffs;

namespace ProgressionReforged.Systems.WhipStats;

// ReSharper disable once InconsistentNaming
public class WhipTagDamageGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private float _tagDamageMult = 1f;

    public void ApplyMultiplier(float mult)
    {
        _tagDamageMult = MathF.Max(mult, 1f);
    }

    public override void ResetEffects(NPC npc)
    {
        if (!npc.HasBuff<PrefixWhipTagDamageBuff>())
        {
            _tagDamageMult = 1f;
        }
    }

    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if (_tagDamageMult == 1f)
            return;

        if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
            return;

        float projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
        modifiers.ScalingBonusDamage += (_tagDamageMult - 1f) * projTagMultiplier;
    }
}