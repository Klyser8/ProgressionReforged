using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.UseTime;

public class SluggishPrefix() : LeveledPrefix(-1, "useTime")
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
        useTimeMult = 1.15f; // 15% slower use time
    }

    public override int GetNext()
    {
        return ModContent.GetInstance<SnappyPrefix>().Type; // Next prefix is Snappy
    }

    public override int GetPrevious()
    {
        return -1; // No previous prefix, as this is the lowest level
    }
}