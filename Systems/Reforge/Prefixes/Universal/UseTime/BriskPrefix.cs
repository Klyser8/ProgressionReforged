using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.UseTime;

public class BriskPrefix() : LeveledPrefix(1, "useTime")
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
        useTimeMult = 0.89f; // 11% faster use time
        base.SetStats(
            ref damageMult, 
            ref knockbackMult, 
            ref useTimeMult, 
            ref scaleMult, 
            ref shootSpeedMult, 
            ref manaMult, 
            ref critBonus);
    }

    public override int GetNext()
    {
        return ModContent.GetInstance<NimblePrefix>().Type; // Next prefix is Snappy
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<SnappyPrefix>().Type;
    }
}