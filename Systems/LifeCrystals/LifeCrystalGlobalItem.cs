using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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

        if (available >= required)
        {
            return true;
        }

        if (player.whoAmI == Main.myPlayer && Main.netMode != NetmodeID.Server)
        {
            string message = Language.GetTextValue("Mods.ProgressionReforged.LifeCrystals.NotEnough", required);
            Main.NewText(message, 255, 240, 20);
        }

        return false;
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

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item.type != ItemID.LifeCrystal)
        {
            return;
        }

        Player player = Main.LocalPlayer;
        if (player is null)
        {
            return;
        }

        int required = LifeCrystalSystem.GetLifeCrystalCostForNextHeart(player);
        int available = player.CountItem(ItemID.LifeCrystal);

        string text = Language.GetTextValue("Mods.ProgressionReforged.LifeCrystals.Tooltip.NextCost", required, available);
        tooltips.Add(new TooltipLine(Mod, "ProgressionReforged_LifeCrystalCost", text));
    }
}
