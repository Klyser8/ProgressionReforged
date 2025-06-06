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
        if (pre < 0)        // -1/-2/-3 mean special roll modes
            return true;    // keep default behavior

        ModPrefix prefix = PrefixLoader.GetPrefix(pre);
        if (prefix == null) // safety check
            return true;

        if (prefix.Mod == null)
            return false;

        if (prefix is LeveledPrefix lp)
            return lp.GetLevel() >= -1 && lp.GetLevel() <= 1;

        return true;
    }

    
    
    
}