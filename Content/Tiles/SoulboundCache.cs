using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ProgressionReforged.Content.Tiles;

public class SoulboundCache : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        TileObjectData.newTile.Height = 1;
        TileObjectData.newTile.Width = 3;
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
        TileObjectData.addTile(Type);

        DustType = DustID.Firework_Green;
        TileID.Sets.DisableSmartCursor[Type] = true;

        AddMapEntry(new Color(200, 200, 200), Language.GetText("Mods.ProgressionReforged.Mediumcore.MapEntry"));
    }

    public override bool RightClick(int i, int j)
    {
        // look for projectile at this tile position
        foreach (Projectile proj in Main.projectile)
        {
            if (proj.active && proj.type == ModContent.ProjectileType<Projectiles.SoulboundCache>())
            {
                if (proj.ModProjectile is Projectiles.SoulboundCache mp && TryMatchTile(mp, i, j))
                {
                    mp.Interact(Main.LocalPlayer);
                    return true;
                }
            }
        }
        return false;
    }
    
    
    public override void MouseOver(int i, int j)
    {
        foreach (Projectile proj in Main.projectile)
        {
            if (proj.active && proj.type == ModContent.ProjectileType<Projectiles.SoulboundCache>())
            {
                if (proj.ModProjectile is Projectiles.SoulboundCache mp && TryMatchTile(mp, i, j))
                {
                    string text = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.ContainerHover", mp.Owner, mp.Value);
                    Main.instance.MouseText(text);
                    Main.LocalPlayer.noThrow = 2;
                    return;
                }
            }
        }
    }

    private static bool TryMatchTile(Projectiles.SoulboundCache cache, int i, int j)
    {
        Vector2 target = cache.TargetPosition;
        if (target == Vector2.Zero && cache.Projectile.ai[1] == 0f)
            return false;

        int tileX = (int)Math.Round(target.X / 16f - 0.5f);
        int tileY = (int)Math.Round((target.Y + 22f) / 16f + 2f);

        return tileX == i && tileY == j - 2;
    }
}