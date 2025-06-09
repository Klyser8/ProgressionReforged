using ProgressionReforged.Systems.Reforge.Prefixes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ProgressionReforged.Systems.Reforge;

public class VanillaPrefixTweaker : GlobalItem
{
    
    // This is used to bypass the leveled prefix level check, allowing all leveled prefixes to be applied
    internal static bool BypassLevelCheck;
    public override bool AllowPrefix(Item item, int pre)
    {
        // Reject vanilla prefixes
        ModPrefix p = PrefixLoader.GetPrefix(pre);
        if (p == null || p.Mod == null)
            return false;

        // Allow only –1, 0, +1 of the Leveled prefixes chain
        if (!BypassLevelCheck && p is LeveledPrefix lp)
            return lp.GetLevel() is >= -1 and <= 1;

        // Allow all modded prefixes otherwise
        return true;
    }

    public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
    {
        // Get the current prefix, if it is an instance of LeveledPrefix
        ModPrefix currentPrefix = PrefixLoader.GetPrefix(item.prefix);
        if (currentPrefix is LeveledPrefix leveledPrefix)
        {
            int reforgeCost = ContentSamples.ItemsByType[item.type].value / 2;
            // Apply price weights based on the leveled prefix stats
            float delta = 
                WeightedDelta(leveledPrefix.DamageMult, PriceWeight.Damage) +
                WeightedDelta(leveledPrefix.UseTimeMult, PriceWeight.UseSpeed, inverse: true) +
                WeightedDelta(leveledPrefix.ShootSpeedMult, PriceWeight.ShootSpeed) +
                WeightedDelta(leveledPrefix.ScaleMult, PriceWeight.Size) +
                WeightedDelta(leveledPrefix.KnockbackMult, PriceWeight.Knockback) +
                WeightedDelta(leveledPrefix.ManaMult, PriceWeight.ManaCost, inverse: true) +
                leveledPrefix.CritBonus / 100f * PriceWeight.CritChance +
                (leveledPrefix.CritDamageMultInternal - 1f) * PriceWeight.CritDamage;
            
            // Apply the delta to the reforge cost
            reforgeCost = (int)(reforgeCost * (1f + delta));
            // Multiply by the leveled prefix level to get the final reforge price
            reforgeCost = (int)(reforgeCost * leveledPrefix.GetLevel() switch
            {
                -1 => 0.75f,
                0 => 1.00f,
                1 => 2.00f,
                2 => 3.00f,
                3 => 4.00f,
                _ => 1.00f
            });
            
            // Set the reforge price
            reforgePrice = reforgeCost;
            return true;
        }

        // If the prefix is not a LeveledPrefix, use the default reforge price
        return false;
    }
    
    private static float WeightedDelta(float mult, float weight, bool inverse = false)
    {
        float delta = inverse ? (1f / mult - 1f) : (mult - 1f);
        return delta * weight;
    }
    
    // 1.00f means “1 % of this stat = 1 % price change”
    // 0.50f ⇒ a 1 % boost adds only 0.5 % cost
    // 2.00f ⇒ a 1 % boost adds 2 % cost
    private static class PriceWeight {
        public const float Damage      = 2.50f;
        public const float UseSpeed    = 3.25f;   // (inverse of useTime)
        public const float ShootSpeed  = 0.80f;   // bullet velocity
        public const float Size        = 1.50f;
        public const float Knockback   = 1.33f;
        public const float ManaCost    = 1.22f;   // inverse
        public const float CritChance  = 4.00f;   // +1 % crit chance
        public const float CritDamage  = 2.22f;   // your new stat
    }
}