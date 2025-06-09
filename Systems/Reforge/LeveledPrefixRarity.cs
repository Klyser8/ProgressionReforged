using ProgressionReforged.Systems.Reforge.Prefixes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace ProgressionReforged.Systems.Reforge;

public class LeveledPrefixRarity : GlobalItem
{
    private static void AdjustRarity(Item item)
    {
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is LeveledPrefix lp)
        {
            int baseRarity = ContentSamples.ItemsByType[item.type].rare;
            int rarityChange = lp.GetLevel() switch
            {
                -1 => -1,
                2 => 1,
                3 => 2,
                _ => 0
            };

            item.rare = baseRarity + rarityChange;
        }
    }

    public override void UpdateInventory(Item item, Player player)
    {
        AdjustRarity(item);
    }

    public override void PostReforge(Item item)
    {
        AdjustRarity(item);
    }

    public override void OnSpawn(Item item, IEntitySource source)
    {
        AdjustRarity(item);
    }
}