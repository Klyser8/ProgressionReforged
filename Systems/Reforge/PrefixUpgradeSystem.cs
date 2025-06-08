using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProgressionReforged.Systems.Reforge.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ProgressionReforged.Systems.Reforge;

internal class PrefixUpgradeSystem : ModSystem
{
    internal UserInterface UpgradeInterface;
    internal PrefixUpgradeUI UpgradeUI;
    internal static PrefixUpgradeSystem Instance;

    public override void Load()
    {
        Instance = this;
        if (!Main.dedServ)
        {
            UpgradeUI = new PrefixUpgradeUI();
            UpgradeUI.Activate();
            UpgradeInterface = new UserInterface();
        }
    }

    public override void Unload()
    {
        Instance = null;
    }

    public void Show()
    {
        if (UpgradeInterface != null)
        {
            Main.playerInventory = true;
            UpgradeInterface.SetState(UpgradeUI);
        }
    }

    public void Hide()
    {
        UpgradeInterface?.SetState(null);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (UpgradeInterface?.CurrentState != null)
        {
            UpgradeInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        if (inventoryIndex != -1)
        {
            layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                "ProgressionReforged: PrefixUpgrade",
                () =>
                {
                    if (UpgradeInterface?.CurrentState != null)
                    {
                        UpgradeInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}