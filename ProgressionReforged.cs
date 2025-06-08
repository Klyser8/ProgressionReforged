using Terraria.ModLoader;

namespace ProgressionReforged;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class ProgressionReforged : Mod
{
    public static ProgressionReforged Instance()
    {
        return ModContent.GetInstance<ProgressionReforged>();
    }
}