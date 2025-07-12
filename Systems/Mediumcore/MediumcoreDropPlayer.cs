using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProgressionReforged.Content.Projectiles;
using ProgressionReforged.Systems.MediumcoreDeath;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ProgressionReforged.Systems.Mediumcore;

internal class MediumcoreDropPlayer : ModPlayer
{
    internal TagCompound? StoredData;

    internal bool HasStoredData => StoredData != null;
    private bool _tempSoftcore;
    public const int MaxInteractDistance = 200; // distance in pixels to interact with a Soulbound Cache
    
    
    public override void PreUpdate()
    {
        if (Player.whoAmI != Main.myPlayer)
            return;

        if (Player.dead)
            return;

        Point mouse = Main.MouseWorld.ToPoint();
        foreach (Projectile proj in Main.projectile)
        {
            if (!proj.active || proj.type != ModContent.ProjectileType<SoulboundCache>())
                continue;

            if (proj.Hitbox.Contains(mouse) && Vector2.Distance(Player.Center, proj.Center) <= 200f)
            {
                if (proj.ModProjectile is SoulboundCache mp)
                {
                    string text = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.ContainerHover", mp.Owner, mp.Value);
                    Main.instance.MouseText(text);
                    Player.noThrow = 2;

                    if (Main.mouseRight && Main.mouseRightRelease)
                    {
                        mp.Interact(Player);
                    }
                }
                break;
            }
        }
    }



    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        if (Player.difficulty != PlayerDifficultyID.MediumCore)
            return true;

        StoredData = CapturePlayerState(Player);
        MediumcoreDropSystem.Instance?.SpawnDeathContainer(Player, StoredData);
        StoredData = null;

        ClearPlayerItems(Player);
        Player.difficulty = PlayerDifficultyID.SoftCore;
        _tempSoftcore = true;

        return true;
    }

    public override void OnRespawn()
    {
        if (_tempSoftcore)
        {
            Player.difficulty = PlayerDifficultyID.MediumCore;
            _tempSoftcore = false;
        }
    }

    internal static TagCompound CapturePlayerState(Player player)
    {
        var tag = new TagCompound
        {
            ["inventory"] = SaveItemArray(player.inventory),
            ["miscEquips"] = SaveItemArray(player.miscEquips),
            ["miscDyes"] = SaveItemArray(player.miscDyes)
        };
        
        
        int value = 0;
        foreach (var item in player.inventory)
            if (!item.IsAir)
                value += item.value * item.stack;
        foreach (var item in player.miscEquips)
            if (!item.IsAir)
                value += item.value * item.stack;
        foreach (var item in player.miscDyes)
            if (!item.IsAir)
                value += item.value * item.stack;

        var loadouts = new List<TagCompound>();
        foreach (var loadout in player.Loadouts)
        {
            var lt = new TagCompound
            {
                ["armor"] = SaveItemArray(loadout.Armor),
                ["dye"] = SaveItemArray(loadout.Dye)
            };
            loadouts.Add(lt);
            
            foreach (var item in loadout.Armor)
                if (!item.IsAir)
                    value += item.value * item.stack;
            foreach (var item in loadout.Dye)
                if (!item.IsAir)
                    value += item.value * item.stack;
        }
        tag["loadouts"] = loadouts;
        tag["value"] = value;
        return tag;
    }

    private static List<TagCompound> SaveItemArray(IReadOnlyList<Item> items)
    {
        var list = new List<TagCompound>(items.Count);
        foreach (var item in items)
            list.Add(ItemIO.Save(item));
        return list;
    }
    
    private static void ClearPlayerItems(Player player)
    {
        foreach (var item in player.inventory)
            item.TurnToAir();
        foreach (var item in player.miscEquips)
            item.TurnToAir();
        foreach (var item in player.miscDyes)
            item.TurnToAir();
        foreach (var loadout in player.Loadouts)
        {
            foreach (var item in loadout.Armor)
                item.TurnToAir();
            foreach (var item in loadout.Dye)
                item.TurnToAir();
        }
    }
}