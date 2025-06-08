using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ProgressionReforged;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class ProgressionReforged : Mod
{
    public static ProgressionReforged Instance()
    {
        return ModContent.GetInstance<ProgressionReforged>();
    }

    public override void Load()
    {
        TextureAssets.Reforge[0] = ModContent.Request<Texture2D>("ProgressionReforged/Content/UI/ReforgeRerollButton");
        TextureAssets.Reforge[1] = ModContent.Request<Texture2D>("ProgressionReforged/Content/UI/ReforgeRerollButtonHovered");
    }
}