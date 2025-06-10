using System;
using System.Collections.Generic;
using ProgressionReforged.Systems.CritDamage;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;

public abstract class CritDamagePrefix(int level, string chainKey) : LeveledPrefix(level, chainKey), ICritDamageProvider
{
    // subclasses just override this property to pick their own value
    public abstract float CritDamageMult { get; }

    public override void SetStats(ref float _damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
	    ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
	    CritDamageMultInternal = CritDamageMult; // assign the crit damage multiplier to the internal field, for price calculation
	    base.SetStats(ref _damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
	    int percent = (int)MathF.Round((CritDamageMult - 1f) * 100f);
	    yield return new TooltipLine(Mod, "PrefixCritDamage",
			    CritDamageTooltip.Format(percent))   // <-- pass % here
		    {
			    IsModifier     = true,
			    IsModifierBad  = percent < 0
		    };
    }

	public static LocalizedText CritDamageTooltip { get; private set; }

	public LocalizedText AdditionalTooltip => this.GetLocalization(nameof(AdditionalTooltip));

	public override void SetStaticDefaults() {
		CritDamageTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(CritDamageTooltip)}");
	}
}