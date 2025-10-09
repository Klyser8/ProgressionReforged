using Microsoft.Xna.Framework;
using ProgressionReforged.Systems.LifeCrystals;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Life;

internal sealed class LifeCrystalCountCommand : ModCommand
{
    public override CommandType Type => CommandType.World;

    public override string Command => "countlifecrystals";

    public override string Usage => "/countlifecrystals";

    public override string Description => "Displays the number of life crystals currently in the world.";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        int count = LifeCrystalSystem.CountLifeCrystals();
        caller.Reply($"There are {count} life crystals in the world.", Color.Orange);
    }
}