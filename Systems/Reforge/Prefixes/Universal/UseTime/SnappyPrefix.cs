using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.UseTime;

public class SnappyPrefix() : LeveledPrefix(0, "useTime")
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
        useTimeMult = 0.95f; // 5% faster use time
    }

    public override int GetNext()
    {
        return ModContent.GetInstance<BriskPrefix>().Type; // Next prefix is Brisk
    }

    public override int GetPrevious()
    {
        return ModContent.GetInstance<SluggishPrefix>().Type;
    }
}