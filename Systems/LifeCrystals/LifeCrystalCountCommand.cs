using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.LifeCrystals;

internal sealed class LifeCrystalCountCommand : ModCommand
{
    public override CommandType Type => CommandType.World;

    public override string Command => "countlifecrystals";

    public override string Usage => "/countlifecrystals";

    public override string Description => "Displays the number of life crystals currently in the world.";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        int count = LifeCrystalSystem.CountLifeCrystals();
        string worldCount = Language.GetTextValue("Mods.ProgressionReforged.LifeCrystals.Command.WorldCount", count);
        caller.Reply(worldCount, Color.Orange);

        if (caller.Player is Player player)
        {
            int required = LifeCrystalSystem.GetLifeCrystalCostForNextHeart(player);
            int available = player.CountItem(ItemID.LifeCrystal);
            string nextCost = Language.GetTextValue("Mods.ProgressionReforged.LifeCrystals.Command.NextCost", required, available);
            caller.Reply(nextCost, Color.Orange);
        }
    }
}
