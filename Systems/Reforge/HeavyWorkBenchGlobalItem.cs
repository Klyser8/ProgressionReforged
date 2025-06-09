using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace ProgressionReforged.Systems.Reforge;

public class HeavyWorkBenchGlobalItem : GlobalItem
{
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item.type == ItemID.HeavyWorkBench)
        {
            string text = Language.GetTextValue("Mods.ProgressionReforged.HeavyWorkBenchTooltip");
            tooltips.Add(new TooltipLine(Mod, "HeavyWorkBenchTooltip", text));
        }
    }
}