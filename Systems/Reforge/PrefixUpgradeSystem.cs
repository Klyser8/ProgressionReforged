using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProgressionReforged.Systems.Reforge.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace ProgressionReforged.Systems.Reforge;

internal class PrefixUpgradeSystem : ModSystem
{
    internal UserInterface UpgradeInterface;
    internal PrefixUpgradeUI UpgradeUI;
    internal static PrefixUpgradeSystem Instance;
    private Point16 _benchPos;

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
    
    public override void PreSaveAndQuit()
    {
        if (UpgradeInterface?.CurrentState is PrefixUpgradeUI ui && !ui.ItemSlotWrapper.Item.IsAir)
        {
            Player player = Main.LocalPlayer;
            player.QuickSpawnItem(Entity.GetSource_NaturalSpawn(), ui.ItemSlotWrapper.Item);
            ui.ItemSlotWrapper.Item.TurnToAir();
        }
        Hide();
    }

    public override void OnWorldUnload()
    {
        PreSaveAndQuit();
    }


    public void Show(int i, int j)
    {
        if (UpgradeInterface != null)
        {
            CloseOtherInterfaces();
            _benchPos = new Point16(i, j);
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
            if (!Main.playerInventory || !PlayerIsNearBench() || AnotherInterfaceOpen())
            {
                Hide();
                return;
            }

            UpgradeInterface.Update(gameTime);
        }
    }
    
    private bool PlayerIsNearBench()
    {
        Vector2 playerPos = Main.LocalPlayer.Center / 16f;
        return Vector2.Distance(playerPos, _benchPos.ToVector2()) <= 6f;
    }

    
    private static bool AnotherInterfaceOpen()
    {
        Player player = Main.LocalPlayer;
        return player.chest != -1 || player.talkNPC != -1 || player.sign != -1 ||
               Main.npcShop > 0 || Main.editChest || Main.editSign;
    }
    
    private static void CloseOtherInterfaces()
    {
        Player player = Main.LocalPlayer;
        player.chest = -1;
        if (player.talkNPC != -1)
            player.SetTalkNPC(-1);
        if (player.sign != -1)
            player.CloseSign();
        if (Main.npcShop > 0)
            Main.SetNPCShopIndex(0);
        Main.editChest = false;
        Main.editSign = false;
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