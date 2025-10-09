using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.LifeCrystals;

internal sealed class LifeCrystalGlobalItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.LifeCrystal;
    }

    public override bool CanUseItem(Item item, Player player)
    {
        if (!base.CanUseItem(item, player))
        {
            return false;
        }

        if (!player.CanConsumeLifeCrystal())
        {
            return false;
        }

        int required = LifeCrystalSystem.GetLifeCrystalCostForNextHeart(player);
        player.GetModPlayer<LifeCrystalPlayer>().PendingLifeCrystalCost = required;
        int available = player.CountItem(ItemID.LifeCrystal);

        return available >= required;
    }

    public override void OnConsumeItem(Item item, Player player)
    {
        if (item.type != ItemID.LifeCrystal)
        {
            return;
        }

        int required = Math.Max(0, player.GetModPlayer<LifeCrystalPlayer>().PendingLifeCrystalCost - 1);
        player.GetModPlayer<LifeCrystalPlayer>().PendingLifeCrystalCost = 1;

        if (required <= 0)
        {
            return;
        }

        player.ConsumeItem(ItemID.LifeCrystal, required);
    }
}
