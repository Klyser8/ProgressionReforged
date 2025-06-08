using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using ProgressionReforged.Systems.Reforge.Prefixes;
using ReLogic.Content;

namespace ProgressionReforged.Systems.Reforge.UI;

internal class PrefixUpgradeUI : UIState
{
    private VanillaItemSlotWrapper _itemSlot;
    private bool _tickPlayed;
    
    private static Asset<Texture2D>[] _upgradeButtonTexture;

    public override void OnInitialize()
    {
        _itemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
        {
            Left = { Pixels = 50 },
            Top = { Pixels = 270 },
            ValidItemFunc = _ => true
        };
        Append(_itemSlot);
        
        _upgradeButtonTexture = new[]{
            ModContent.Request<Texture2D>("ProgressionReforged/Content/UI/ReforgeUpgradeButton", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>("ProgressionReforged/Content/UI/ReforgeUpgradeButtonHovered", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>("ProgressionReforged/Content/UI/ReforgeUpgradeButtonDisabled", AssetRequestMode.ImmediateLoad)
        };
    }

    public override void OnDeactivate()
    {
        if (!_itemSlot.Item.IsAir)
        {
            Main.LocalPlayer.QuickSpawnItem(Entity.GetSource_NaturalSpawn(),_itemSlot.Item);
            _itemSlot.Item.TurnToAir();
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        Main.hidePlayerCraftingMenu = true;

        const int SlotX = 50;
        const int SlotY = 270;

        if (_itemSlot.Item.IsAir)
        {
            string message = Language.GetTextValue("Mods.ProgressionReforged.PrefixUpgrade.PlaceItem");
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, message, new Vector2(SlotX + 50, SlotY),
                new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            return;
        }

        if (PrefixLoader.GetPrefix(_itemSlot.Item.prefix) is not LeveledPrefix leveled)
        {
            string message = Language.GetTextValue("Mods.ProgressionReforged.PrefixUpgrade.NoUpgradablePrefix");
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, message, new Vector2(SlotX + 50, SlotY), Color.Red, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            return;
        }

        int price = PrefixUpgradePrice(_itemSlot.Item, leveled);
        bool atMax = leveled.GetNext() == -1;
        if (atMax)
        {
            string message = Language.GetTextValue("Mods.ProgressionReforged.PrefixUpgrade.MaxLevel");
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, message, new Vector2(SlotX + 50, SlotY), Color.Yellow, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
        }
        else
        {
            string costText = Language.GetTextValue("LegacyInterface.46") + ": ";
            int[] coins = Utils.CoinsSplit(price);
            var coinsText = new StringBuilder();
            string[] coinKeys =
            {
                "LegacyInterface.18",
                "LegacyInterface.17",
                "LegacyInterface.16",
                "LegacyInterface.15"
            };
            Color[] coinColors =
            {
                Colors.CoinCopper,
                Colors.CoinSilver,
                Colors.CoinGold,
                Colors.CoinPlatinum
            };
            for (int i = 3; i >= 0; i--)
            {
                if (coins[i] <= 0)
                    continue;
                if (coinsText.Length > 0)
                    coinsText.Append(" ");
                coinsText.Append($"[c/{coinColors[i].Hex3()}:{coins[i]} {Language.GetTextValue(coinKeys[i])}]");
            }

            ItemSlot.DrawSavings(spriteBatch, SlotX + 130, Main.instance.invBottom, true);
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, costText, new Vector2(SlotX + 50, SlotY),
                new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, coinsText.ToString(), new Vector2(SlotX + 50 + FontAssets.MouseText.Value.MeasureString(costText).X, SlotY),
                Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
        }

        int buttonX = SlotX + 70;
        int buttonY = SlotY + 40;
        bool hovering = Main.mouseX > buttonX - 15 && Main.mouseX < buttonX + 15 && Main.mouseY > buttonY - 15 && Main.mouseY < buttonY + 15 && !PlayerInput.IgnoreMouseInterface;
        bool canUpgrade = !atMax && Main.LocalPlayer.CanAfford(price);
        Texture2D texture = _upgradeButtonTexture[canUpgrade ? (hovering ? 1 : 0) : 2].Value;
        Color drawColor = canUpgrade ? Color.White : Color.White * 0.5f;

        spriteBatch.Draw(texture, new Vector2(buttonX, buttonY), null, drawColor, 0f, texture.Size() / 2f, 1.0f, SpriteEffects.None, 0f);
        
        if (!hovering || !canUpgrade)
        {
            _tickPlayed = false;
            return;
        }
        
        Main.hoverItemName = Language.GetTextValue("Mods.ProgressionReforged.PrefixUpgrade.UpgradeHover", leveled.GetNext());
        if (!_tickPlayed)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
        _tickPlayed = true;
        Main.LocalPlayer.mouseInterface = true;

        if (!Main.mouseLeftRelease || !Main.mouseLeft || !ItemLoader.CanReforge(_itemSlot.Item))
            return;

        Main.LocalPlayer.BuyItem(price);
        bool fav = _itemSlot.Item.favorited;
        int stack = _itemSlot.Item.stack;

        Item newItem = _itemSlot.Item.Clone();
        newItem.SetDefaults(newItem.type);
        newItem.Prefix(leveled.GetNext());

        _itemSlot.Item = newItem;
        _itemSlot.Item.position = Main.LocalPlayer.Center;
        _itemSlot.Item.favorited = fav;
        _itemSlot.Item.stack = stack;

        ItemLoader.PostReforge(_itemSlot.Item);
        SoundEngine.PlaySound(SoundID.Item37);
    }

    private static int PrefixUpgradePrice(Item item, LeveledPrefix leveled)
    {
        int price = ContentSamples.ItemsByType[item.type].value;
        float delta =
            WeightedDelta(leveled.DamageMult, PriceWeight.Damage) +
            WeightedDelta(leveled.UseTimeMult, PriceWeight.UseSpeed, true) +
            WeightedDelta(leveled.ShootSpeedMult, PriceWeight.ShootSpeed) +
            WeightedDelta(leveled.ScaleMult, PriceWeight.Size) +
            WeightedDelta(leveled.KnockbackMult, PriceWeight.Knockback) +
            WeightedDelta(leveled.ManaMult, PriceWeight.ManaCost, true) +
            leveled.CritBonus / 100f * PriceWeight.CritChance +
            (leveled.CritDamageMultInternal - 1f) * PriceWeight.CritDamage;

        price = (int)(price * (1f + delta));
        price = (int)(price * leveled.GetLevel() switch
        {
            -1 => 0.75f,
            0 => 1.00f,
            1 => 2.00f,
            2 => 3.00f,
            3 => 4.00f,
            _ => 1.00f
        });
        return price;
    }

    private static float WeightedDelta(float mult, float weight, bool inverse = false)
    {
        float delta = inverse ? (1f / mult - 1f) : (mult - 1f);
        return delta * weight;
    }

    private static class PriceWeight
    {
        public const float Damage = 2.50f;
        public const float UseSpeed = 3.25f;
        public const float ShootSpeed = 0.80f;
        public const float Size = 1.50f;
        public const float Knockback = 1.33f;
        public const float ManaCost = 1.22f;
        public const float CritChance = 4.00f;
        public const float CritDamage = 2.22f;
    }
}