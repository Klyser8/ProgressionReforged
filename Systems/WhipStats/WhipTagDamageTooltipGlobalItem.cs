using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using ProgressionReforged.Systems.Reforge.Prefixes.Summon;

namespace ProgressionReforged.Systems.WhipStats;

public class WhipTagDamageTooltipGlobalItem : GlobalItem
{
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item.DamageType != DamageClass.SummonMeleeSpeed)
            return;

        if (item.prefix <= 0 || PrefixLoader.GetPrefix(item.prefix) is not IWhipTagDamageProvider provider)
            return;

        for (int i = 0; i < tooltips.Count; i++)
        {
            TooltipLine line = tooltips[i];
            if (line.Mod == "Terraria" && Regex.IsMatch(line.Text, "^\\d+ .*summon tag damage$"))
            {
                Match m = Regex.Match(line.Text, "^\\d+");
                if (m.Success && int.TryParse(m.Value, out int baseVal))
                {
                    int newVal = (int)MathF.Round(baseVal * provider.WhipTagDamageMult);
                    line.Text = Regex.Replace(line.Text, "^\\d+", newVal.ToString());
                }
                break;
            }
        }
    }
}