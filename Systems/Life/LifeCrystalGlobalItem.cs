using System;
using ProgressionReforged.Systems.LifeCrystals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Life;

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

        if (player.ConsumedLifeCrystals >= 15)
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

        player.ConsumeItem(ItemID.LifeCrystal);
    }
}