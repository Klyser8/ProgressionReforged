using System;
using System.Collections.Generic;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Summon;

public abstract class WhipRangePrefix(int level, string chainKey) : LeveledPrefix(level, chainKey), IWhipRangeProvider
{
    // subclasses just override this property to pick their own value
    public abstract float WhipRangeMult { get; }

    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
	    ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
	    WhipRangeMultInternal = WhipRangeMult; // assign the crit damage multiplier to the internal field, for price calculation
	    base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
	    int percent = (int)MathF.Round((WhipRangeMult - 1f) * 100f);
	    yield return new TooltipLine(Mod, "PrefixWhipRange",
			    WhipRangeTooltip.Format(percent))   // <-- pass % here
		    {
			    IsModifier     = true,
			    IsModifierBad  = percent < 0
		    };
    }

	public static LocalizedText WhipRangeTooltip { get; private set; }

	public LocalizedText AdditionalTooltip => this.GetLocalization(nameof(AdditionalTooltip));

	public override void SetStaticDefaults() {
		WhipRangeTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(WhipRangeTooltip)}");
	}
}