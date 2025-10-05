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
                var mp = proj.ModProjectile as Projectiles.SoulboundCache;
                if (mp != null && mp.tileX == i && mp.tileY == j - 2)
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
                var mp = proj.ModProjectile as Projectiles.SoulboundCache;
                if (mp != null && mp.tileX == i && mp.tileY == j - 2)
                {
                    string text = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.ContainerHover", mp.Owner, mp.Value);
                    Main.instance.MouseText(text);
                    Main.LocalPlayer.noThrow = 2;
                    return;
                }
            }
        }
    }
}