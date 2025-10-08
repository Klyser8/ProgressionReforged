using System;
using System.Text;
using Microsoft.Xna.Framework;
using ProgressionReforged.Systems.MediumcoreDeath;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ProgressionReforged.Content.Projectiles;

public class SoulboundCache : ModProjectile
{
    private static readonly string[] CoinTextKeys =
    {
        "LegacyInterface.18",
        "LegacyInterface.17",
        "LegacyInterface.16",
        "LegacyInterface.15"
    };

    private static readonly string[] CoinFallbackNames =
    {
        "Copper",
        "Silver",
        "Gold",
        "Platinum"
    };

    private static readonly Color[] CoinColors =
    {
        Colors.CoinCopper,
        Colors.CoinSilver,
        Colors.CoinGold,
        Colors.CoinPlatinum
    };

    
    internal TagCompound? StoredData;
    internal string Owner = "";
    internal int Value;
    internal Vector2 TargetPosition;
    internal string DropId = string.Empty;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 8;
    }

    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 44;
        Projectile.aiStyle = 0;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.timeLeft = int.MaxValue;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.alpha = 255; // fade in
    }

    public override void AI()
    {
        // movement towards safe spot
        if (Projectile.ai[1] == 0f)
        {
            Vector2 toTarget = TargetPosition - Projectile.Center;
            float distanceSquared = toTarget.LengthSquared();

            // steer towards the target each tick
            if (distanceSquared > 16f)
            {
                if (distanceSquared > 0f)
                {
                    float distance = MathF.Sqrt(distanceSquared);
                    Projectile.velocity = toTarget / distance * 2f;
                }
            }
            else
            {
                Projectile.Center = TargetPosition;
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[1] = 1f;
            }
        }
        MediumcoreDropSystem.Instance?.UpdateDrop(DropId, Projectile.Center, Projectile.ai[1] != 0f);
        // fade in/out
        if (Projectile.ai[0] == 0f)
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
        }
        else
        {
            Projectile.alpha += 15;
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
                return;
            }
        }

        Lighting.AddLight(Projectile.Center, 0.3f, 1f, 0.3f);
        if (Main.rand.NextBool(4))
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, 0f, -0.5f);
        
        Projectile.frameCounter++;
        if (Projectile.frameCounter > 6)
        {
            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Type])
                Projectile.frame = 0;
            Projectile.frameCounter = 0;
        }
    }

    internal void Interact(Player player)
    {
        if (StoredData == null || Projectile.ai[1] == 0f)
            return;

        bool inventoryFull = true;
        for (int i = 0; i < player.inventory.Length; i++)
        {
            if (player.inventory[i].IsAir)
            {
                inventoryFull = false;
                break;
            }
        }

        IEntitySource restoreSource = player.GetSource_Misc("MediumcoreContainer");

        RestorePlayer(player, StoredData, inventoryFull, restoreSource);
        StoredData = null;
        MediumcoreDropSystem.Instance?.RemoveDrop(DropId);
        DropId = string.Empty;
        Projectile.ai[0] = 1f;
        Projectile.velocity = Vector2.Zero;
        for (int i = 0; i < 15; i++)
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, 0f, -0.5f);
    }

    public override void Kill(int timeLeft)
    {
        MediumcoreDropSystem.Instance?.RemoveDrop(DropId);
        DropId = string.Empty;
    }
    
    private static void PlaceItem(Player player, ref Item slot, Item item, bool dropToGround, IEntitySource source)
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


        if (dropToGround)
        {
            Item? spawnedItem = player.QuickSpawnClonedItemDirect(source, item);
            if (spawnedItem != null)
            {
                spawnedItem.velocity = Main.rand.NextVector2Circular(2f, 2f);
            }

            return;
        }

        player.QuickSpawnClonedItem(source, item);
        
    }
    
    private static bool IsCopper(Item item)
    {
        return item.type == ItemID.CopperPickaxe ||
               item.type == ItemID.CopperAxe ||
               item.type == ItemID.CopperShortsword;
    }
    
    internal static void RestorePlayer(Player player, TagCompound data, bool dropToGround, IEntitySource source)
    {
        var inv = data.GetList<TagCompound>("inventory");
        for (int i = 0; i < inv.Count && i < player.inventory.Length; i++)
        {
            var item = ItemIO.Load(inv[i]);
            PlaceItem(player, ref player.inventory[i], item, dropToGround, source);
        }

        var misc = data.GetList<TagCompound>("miscEquips");
        for (int i = 0; i < misc.Count && i < player.miscEquips.Length; i++)
        {
            var item = ItemIO.Load(misc[i]);
            PlaceItem(player, ref player.miscEquips[i], item, dropToGround, source);
        }

        var miscD = data.GetList<TagCompound>("miscDyes");
        for (int i = 0; i < miscD.Count && i < player.miscDyes.Length; i++)
        {
            var item = ItemIO.Load(miscD[i]);
            PlaceItem(player, ref player.miscDyes[i], item, dropToGround, source);
        }

        var loadouts = data.GetList<TagCompound>("loadouts");
        for (int l = 0; l < loadouts.Count && l < player.Loadouts.Length; l++)
        {
            var lt = loadouts[l];
            var armor = lt.GetList<TagCompound>("armor");
            for (int i = 0; i < armor.Count && i < player.Loadouts[l].Armor.Length; i++)
            {
                var item = ItemIO.Load(armor[i]);
                PlaceItem(player, ref player.Loadouts[l].Armor[i], item, dropToGround, source);
            }
            var dye = lt.GetList<TagCompound>("dye");
            for (int i = 0; i < dye.Count && i < player.Loadouts[l].Dye.Length; i++)
            {
                var item = ItemIO.Load(dye[i]);
                PlaceItem(player, ref player.Loadouts[l].Dye[i], item, dropToGround, source);
            }
        }
    }
    
    internal static string BuildHoverText(string owner, long value, bool hasArrived)
    {
        string cacheName = Language.GetTextValue("Mods.ProgressionReforged.Projectiles.SoulboundCache.DisplayName");
        if (string.IsNullOrWhiteSpace(cacheName))
            cacheName = "Soulbound Cache";

        string ownerText;
        if (string.IsNullOrWhiteSpace(owner))
        {
            ownerText = cacheName;
        }
        else
        {
            string localizedOwner = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.SoulboundCacheOwnerFormat", owner, cacheName);
            ownerText = string.IsNullOrWhiteSpace(localizedOwner) ? $"{owner}'s {cacheName}" : localizedOwner;
        }

        if (!hasArrived)
        {
            string travelingText = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.SoulboundCacheTraveling");
            ownerText += string.IsNullOrWhiteSpace(travelingText) ? " (Traveling)" : $" ({travelingText})";
        }

        string coins = FormatCoinValue(value);
        string valueText = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.SoulboundCacheValueLabel", coins);
        if (string.IsNullOrWhiteSpace(valueText))
            valueText = $"Value: {coins}";

        string hoverText = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.ContainerHover", ownerText, valueText);
        if (string.IsNullOrWhiteSpace(hoverText))
            hoverText = $"{ownerText}\n{valueText}";

        return hoverText;
    }

    private static string FormatCoinValue(long value)
    {
        int[] coins = Utils.CoinsSplit(value);
        var builder = new StringBuilder();

        for (int i = 3; i >= 0; i--)
        {
            if (coins[i] == 0)
            {
                continue;
            }
            if (builder.Length > 0)
                builder.Append(' ');

            string coinName = Language.GetTextValue(CoinTextKeys[i]);
            if (string.IsNullOrWhiteSpace(coinName))
                coinName = CoinFallbackNames[i];

            builder.Append($"[c/{CoinColors[i].Hex3()}:{coins[i]} {coinName}]");
        }

        return builder.ToString();
    }
}