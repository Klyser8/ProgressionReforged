using ProgressionReforged.Systems.Reforge.UI;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace ProgressionReforged.Systems.Reforge;

internal class PrefixUpgradePlayer : ModPlayer
{
    
    public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
    {
        if (context == ItemSlot.Context.InventoryItem &&
            PrefixUpgradeSystem.Instance?.UpgradeInterface?.CurrentState is PrefixUpgradeUI ui)
        {
            var wrapper = ui.ItemSlotWrapper;
            Item item = inventory[slot];
            if (wrapper.Item.IsAir && !item.IsAir && (wrapper.ValidItemFunc?.Invoke(item) ?? true))
            {
                wrapper.Item = item.Clone();
                inventory[slot].TurnToAir();
                SoundEngine.PlaySound(SoundID.Grab);
                return true;
            }
        }
        return base.ShiftClickSlot(inventory, context, slot);
    }

    public override bool HoverSlot(Item[] inventory, int context, int slot)
    {
        if (context == ItemSlot.Context.InventoryItem && ItemSlot.ShiftInUse &&
            PrefixUpgradeSystem.Instance?.UpgradeInterface?.CurrentState is PrefixUpgradeUI ui)
        {
            var wrapper = ui.ItemSlotWrapper;
            Item item = inventory[slot];
            if (wrapper.Item.IsAir && !item.IsAir && (wrapper.ValidItemFunc?.Invoke(item) ?? true))
            {
                Main.cursorOverride = 9;
                return true;
            }
        }
        return base.HoverSlot(inventory, context, slot);
    }
}