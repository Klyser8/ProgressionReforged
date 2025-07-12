using System.Collections.Generic;
using System.Linq;
using ProgressionReforged.Systems.MediumcoreDeath;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ProgressionReforged.Content.Items;

public class SoulboundCache : ModItem
{
    internal TagCompound? StoredData;
    internal string Owner = "";
    internal int Value;

    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 32;
        Item.maxStack = 1;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.consumable = true;
    }
    
    public override bool CanUseItem(Player player)
    {
        return StoredData != null;
    }

    public override bool? UseItem(Player player)
    {
        if (StoredData == null || MediumcoreDropSystem.Instance == null)
            return false;

        TagCompound tag = new()
        {
            ["pos"] = player.Center,
            ["data"] = StoredData,
            ["owner"] = string.IsNullOrEmpty(Owner) ? player.name : Owner,
            ["value"] = Value
        };

        MediumcoreDropSystem.SpawnContainerFromTag(tag);

        StoredData = null;
        Owner = "";
        Value = 0;

        return true;
    }
    
    internal static int CalculateValue(TagCompound data)
    {
        int value = 0;

        void AddList(IList<TagCompound> list)
        {
            foreach (var tag in list)
            {
                Item item = ItemIO.Load(tag);
                if (!item.IsAir)
                    value += item.value * item.stack;
            }
        }

        AddList(data.GetList<TagCompound>("inventory"));
        AddList(data.GetList<TagCompound>("miscEquips"));
        AddList(data.GetList<TagCompound>("miscDyes"));

        foreach (var lt in data.GetList<TagCompound>("loadouts"))
        {
            AddList(lt.GetList<TagCompound>("armor"));
            AddList(lt.GetList<TagCompound>("dye"));
        }

        return value;
    }


    internal static void RestorePlayer(Player player, TagCompound data)
    {
        var inv = data.GetList<TagCompound>("inventory");
        for (int i = 0; i < inv.Count && i < player.inventory.Length; i++)
        {
            var item = ItemIO.Load(inv[i]);
            PlaceItem(player, ref player.inventory[i], item);
        }

        var misc = data.GetList<TagCompound>("miscEquips");
        for (int i = 0; i < misc.Count && i < player.miscEquips.Length; i++)
        {
            var item = ItemIO.Load(misc[i]);
            PlaceItem(player, ref player.miscEquips[i], item);
        }

        var miscD = data.GetList<TagCompound>("miscDyes");
        for (int i = 0; i < miscD.Count && i < player.miscDyes.Length; i++)
        {
            var item = ItemIO.Load(miscD[i]);
            PlaceItem(player, ref player.miscDyes[i], item);
        }

        var loadouts = data.GetList<TagCompound>("loadouts");
        for (int l = 0; l < loadouts.Count && l < player.Loadouts.Length; l++)
        {
            var lt = loadouts[l];
            var armor = lt.GetList<TagCompound>("armor");
            for (int i = 0; i < armor.Count && i < player.Loadouts[l].Armor.Length; i++)
            {
                var item = ItemIO.Load(armor[i]);
                PlaceItem(player, ref player.Loadouts[l].Armor[i], item);
            }
            var dye = lt.GetList<TagCompound>("dye");
            for (int i = 0; i < dye.Count && i < player.Loadouts[l].Dye.Length; i++)
            {
                var item = ItemIO.Load(dye[i]);
                PlaceItem(player, ref player.Loadouts[l].Dye[i], item);
            }
        }
    }

    private static void PlaceItem(Player player, ref Item slot, Item item)
    {
        if (item.IsAir)
            return;
        
        if (IsCopper(slot))
            slot.TurnToAir();


        if (slot.IsAir)
        {
            slot = item.Clone();
            return;
        }

        for (int i = 0; i < player.inventory.Length; i++)
        {
            if (player.inventory[i].IsAir)
            {
                player.inventory[i] = item.Clone();
                return;
            }
        }


        player.QuickSpawnClonedItem(player.GetSource_Misc("MediumcoreContainer"), item);
        
    }
    
    private static bool IsCopper(Item item)
    {
        return item.type == ItemID.CopperPickaxe ||
               item.type == ItemID.CopperAxe ||
               item.type == ItemID.CopperShortsword;
    }

    public override void SaveData(TagCompound tag)
    {
        if (StoredData != null)
            tag["data"] = StoredData;
        if (!string.IsNullOrEmpty(Owner))
            tag["owner"] = Owner;
        if (Value > 0)
            tag["value"] = Value;
    }

    public override void LoadData(TagCompound tag)
    {
        if (tag.ContainsKey("data"))
            StoredData = tag.GetCompound("data");
        Owner = tag.GetString("owner");
        Value = tag.GetInt("value");
    }
}