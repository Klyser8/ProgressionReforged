using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.UseTime;

public class NimblePrefix() : LeveledPrefix(2, "useTime")
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    public override void SetStats(
        ref float damageMult, 
        ref float knockbackMult, 
        ref float useTimeMult, 
        ref float scaleMult,
        ref float shootSpeedMult,
        ref float manaMult, 
        ref int critBonus)
    {
        useTimeMult = 0.7f; // 30% faster use time
    }

    public override int GetNext()
    {
        return ModContent.GetInstance<RecklessPrefix>().Type; // Next prefix is Swift
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<BriskPrefix>().Type;
    }
}