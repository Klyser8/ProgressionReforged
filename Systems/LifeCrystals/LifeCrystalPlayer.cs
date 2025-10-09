using Terraria.ModLoader;

namespace ProgressionReforged.Systems.LifeCrystals;

internal sealed class LifeCrystalPlayer : ModPlayer
{
    public int PendingLifeCrystalCost { get; set; } = 1;
}
