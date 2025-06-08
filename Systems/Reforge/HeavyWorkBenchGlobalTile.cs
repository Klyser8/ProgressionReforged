using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge;

public class HeavyWorkBenchGlobalTile : GlobalTile
{
    public override void RightClick(int i, int j, int type)
    {
        if (type == TileID.HeavyWorkBench)
        {
            PrefixUpgradeSystem.Instance?.Show(i, j);
        }
    }
    
    public override void MouseOver(int i, int j, int type)
    {
        if (type == TileID.HeavyWorkBench)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemID.HeavyWorkBench;
        }
    }
}