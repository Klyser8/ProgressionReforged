using ProgressionReforged.Systems.Reforge.Prefixes;
using ProgressionReforged.Systems.Reforge.Prefixes.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace ProgressionReforged.Systems.Reforge;

public class VanillaPrefixTweaker : GlobalItem
{
    
    // This is used to bypass the leveled prefix level check, allowing all leveled prefixes to be applied
    internal static bool BypassLevelCheck;
    public override bool AllowPrefix(Item item, int prefixType)
    {
        // Reject vanilla prefixes
        ModPrefix modPrefix = PrefixLoader.GetPrefix(prefixType);
        if (modPrefix?.Mod == null)
            return false;

        // When adding prefixes through normal reforging or drops we only want
        // the first tier of the leveled prefix chain. However items that
        // already have a higher tier prefix should not lose it when the game
        // reloads.  If the item already has this prefix, always allow it so it
        // persists across save/load cycles. Otherwise restrict the random roll
        // to the first tier unless bypassed by the upgrade UI.
        if (modPrefix is LeveledPrefix leveledPrefix)
        {
            if (item.prefix == prefixType)
                return true;

            if (!BypassLevelCheck)
                return leveledPrefix.GetLevel() is >= -1 and <= 1;
        }

        // Allow all modded prefixes otherwise
        return true;
    }

    public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
    {
        // Get the current prefix, if it is an instance of LeveledPrefix
        ModPrefix currentPrefix = PrefixLoader.GetPrefix(item.prefix);
        
        // If the item has no prefix, reforge cost should be half of its current value
        if (item.prefix == 0)
        {
            reforgePrice = (int)(item.value * 0.5f);
            return true;
        }

        if (currentPrefix is LeveledPrefix leveledPrefix)
        {
            int baseValue = ContentSamples.ItemsByType[item.type].value;
            float levelMult = leveledPrefix.GetLevel() switch
            {
                -1 => 0.9f,
                0 => 1.00f,
                1 => 1.50f,
                2 => 2.00f,
                3 => 3.00f,
                _ => 1.00f
            };
            
            int reforgeCost;
            if (leveledPrefix is AccessoryPrefix acc)
            {
                float weight = AccessoryPriceConfig.GetWeight(acc);
                reforgeCost = (int)(baseValue * weight * levelMult);
            }
            else
            {
                float delta =
                    PriceHelper.WeightedDelta(leveledPrefix.DamageMult, PriceHelper.PriceWeight.Damage) +
                    PriceHelper.WeightedDelta(leveledPrefix.UseTimeMult, PriceHelper.PriceWeight.UseSpeed, inverse: true) +
                    PriceHelper.WeightedDelta(leveledPrefix.ShootSpeedMult, PriceHelper.PriceWeight.ShootSpeed) +
                    PriceHelper.WeightedDelta(leveledPrefix.ScaleMult, PriceHelper.PriceWeight.Size) +
                    PriceHelper.WeightedDelta(leveledPrefix.KnockbackMult, PriceHelper.PriceWeight.Knockback, inverse: true) +
                    PriceHelper.WeightedDelta(leveledPrefix.ManaMult, PriceHelper.PriceWeight.ManaCost, inverse: true) +
                    leveledPrefix.CritBonus / 100f * PriceHelper.PriceWeight.CritChance +
                    (leveledPrefix.CritDamageMultInternal - 1f) * PriceHelper.PriceWeight.CritDamage +
                    (leveledPrefix.WhipRangeMultInternal - 1f) * PriceHelper.PriceWeight.WhipRange +
                    (leveledPrefix.WhipTagDamageMultInternal - 1f) * PriceHelper.PriceWeight.WhipTagDamage;

                reforgeCost = (int)(baseValue * (1f + delta) * levelMult);
            }
            
            
            // Ensure the reforge price never drops below half of the unmodified item's value
            int minPrice = (int)(ContentSamples.ItemsByType[item.type].value * 0.5f);
            if (reforgeCost < minPrice)
                reforgeCost = minPrice;

            
            // Set the reforge price
            reforgePrice = reforgeCost;
            return true;
        }

        // If the prefix is not a LeveledPrefix, use the default reforge price
        return false;
    }
    
    public override void SaveData(Item item, TagCompound tag)
    {
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is LeveledPrefix)
            tag["SavedLeveledPrefix"] = item.prefix;
    }

    public override void LoadData(Item item, TagCompound tag)
    {
        if (tag.ContainsKey("SavedLeveledPrefix"))
        {
            int saved = tag.GetInt("SavedLeveledPrefix");
            if (saved > 0 && PrefixLoader.GetPrefix(saved) is LeveledPrefix)
            {
                if (item.prefix != saved)
                {
                    BypassLevelCheck = true;
                    item.Prefix(saved);
                    BypassLevelCheck = false;
                }
            }
        }
    }
}