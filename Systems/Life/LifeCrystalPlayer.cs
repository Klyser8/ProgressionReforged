using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Life;

internal sealed class LifeCrystalPlayer : ModPlayer
{
    public int PendingLifeCrystalCost { get; set; } = 1;
}