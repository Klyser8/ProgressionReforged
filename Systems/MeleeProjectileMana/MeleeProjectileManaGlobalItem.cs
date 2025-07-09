using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.MeleeProjectileMana;

public class MeleeProjectileManaGlobalItem : GlobalItem
{
    private const float DamageFactor = 0.1f;
    private const float UseTimeFactor = 0.05f;
    private const float ScaleFactor = 1.5f;

    private static int GetManaCost(Item item)
    {
        if (item.ModItem is IMeleeProjectileManaCostProvider provider)
            return provider.ProjectileManaCost;

        float cost = item.damage * DamageFactor + item.useTime * UseTimeFactor + item.scale * ScaleFactor;
        return Math.Max(1, (int)MathF.Round(cost));
    }

    public override bool AppliesToEntity(Item item, bool lateInstantiation)
    {
        return item.DamageType == DamageClass.Melee && item.shoot > ProjectileID.None;
    }

    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int cost = GetManaCost(item);
        if (!player.CheckMana(cost, true))
            return false;

        return true;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        int cost = GetManaCost(item);
        string text = Language.GetTextValue("Mods.ProgressionReforged.MeleeProjectileManaTooltip", cost);
        tooltips.Add(new TooltipLine(Mod, "MeleeProjectileManaTooltip", text));
    }
}
