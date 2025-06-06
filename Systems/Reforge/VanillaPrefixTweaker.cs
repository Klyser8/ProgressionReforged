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
        // Grab the actual prefix object
        ModPrefix prefix = PrefixLoader.GetPrefix(pre);

        // 1) Block ALL vanilla prefixes outright
        if (prefix.Mod == null)                  // vanilla = no owning mod
            return false;                        // → tML keeps searching

        // 2) If it’s one of *your* LeveledPrefixes, allow only tiers −1 .. +1
        if (prefix is LeveledPrefix lp)
            return lp.GetLevel() >= -1 && lp.GetLevel() <= 1;   // true for -1/0/+1

        // 3) For every other mod’s prefix, just let it through
        return true;
    }
    
    
    
}