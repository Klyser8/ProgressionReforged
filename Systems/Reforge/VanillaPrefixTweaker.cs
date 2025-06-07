using ProgressionReforged.Systems.Reforge.Prefixes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ProgressionReforged.Systems.Reforge;

public class VanillaPrefixTweaker : GlobalItem
{
    public override bool AllowPrefix(Item item, int pre)
    {
        // Reject vanilla prefixes
        ModPrefix p = PrefixLoader.GetPrefix(pre);
        if (p == null || p.Mod == null)
            return false;

        // Allow only –1, 0, +1 of the Leveled prefixes chain
        if (p is LeveledPrefix lp)
            return lp.GetLevel() is >= -1 and <= 3;

        // Allow every other mod’s prefixes
        return true;
    }

}