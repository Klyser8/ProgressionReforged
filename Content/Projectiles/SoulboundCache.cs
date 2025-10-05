using System;
using Microsoft.Xna.Framework;
using ProgressionReforged.Systems.MediumcoreDeath;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ProgressionReforged.Content.Projectiles;

public class SoulboundCache : ModProjectile
{
    internal TagCompound? StoredData;
    internal string Owner = "";
    internal int Value;
    internal Vector2 TargetPosition;
    internal string DropId = string.Empty;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 8;
    }

    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 44;
        Projectile.aiStyle = 0;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.timeLeft = int.MaxValue;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.alpha = 255; // fade in
    }

    public override void AI()
    {
        // movement towards safe spot
        if (Projectile.ai[1] == 0f)
        {
            Vector2 toTarget = TargetPosition - Projectile.Center;
            float distanceSquared = toTarget.LengthSquared();

            // steer towards the target each tick
            if (distanceSquared > 16f)
            {
                if (distanceSquared > 0f)
                {
                    float distance = MathF.Sqrt(distanceSquared);
                    Projectile.velocity = toTarget / distance * 2f;
                }
            }
            else
            {
                Projectile.Center = TargetPosition;
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[1] = 1f;
            }
        }
        MediumcoreDropSystem.Instance?.UpdateDrop(DropId, Projectile.Center, Projectile.ai[1] != 0f);
        // fade in/out
        if (Projectile.ai[0] == 0f)
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
        }
        else
        {
            Projectile.alpha += 15;
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
                return;
            }
        }

        Lighting.AddLight(Projectile.Center, 0.3f, 1f, 0.3f);
        if (Main.rand.NextBool(4))
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, 0f, -0.5f);
        
        Projectile.frameCounter++;
        if (Projectile.frameCounter > 6)
        {
            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Type])
                Projectile.frame = 0;
            Projectile.frameCounter = 0;
        }
    }

    internal void Interact(Player player)
    {
        if (StoredData == null || Projectile.ai[1] == 0f)
            return;

        Items.SoulboundCache.RestorePlayer(player, StoredData);
        StoredData = null;
        MediumcoreDropSystem.Instance?.RemoveDrop(DropId);
        DropId = string.Empty;
        Projectile.ai[0] = 1f;
        Projectile.velocity = Vector2.Zero;
        for (int i = 0; i < 15; i++)
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, 0f, -0.5f);
    }

    public override void Kill(int timeLeft)
    {
        MediumcoreDropSystem.Instance?.RemoveDrop(DropId);
        DropId = string.Empty;
    }
}
