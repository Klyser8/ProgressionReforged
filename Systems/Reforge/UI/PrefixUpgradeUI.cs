using System;
using System.Text;
using System.Collections.Generic;
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
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using ReLogic.Content;

namespace ProgressionReforged.Systems.Reforge.UI;

internal class PrefixUpgradeUI : UIState
{
    private VanillaItemSlotWrapper _itemSlot;
    private bool _tickPlayed;
    
    private static Asset<Texture2D>[] _upgradeButtonTexture;

    internal VanillaItemSlotWrapper ItemSlotWrapper => _itemSlot;

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
            Player player = Main.LocalPlayer;
            if (!_itemSlot.Item.IsAir)
                player.QuickSpawnClonedItemDirect(Entity.GetSource_NaturalSpawn(), _itemSlot.Item);
            _itemSlot.Item.TurnToAir();
        }
    }
    
    
    internal bool SwapItem(ref Item item)
    {
        if (_itemSlot.ValidItemFunc != null && !_itemSlot.ValidItemFunc(item))
            return false;

        Utils.Swap(ref _itemSlot.Item, ref item);
        return true;
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
        
        int nextType = GetNextUsefulPrefix(_itemSlot.Item, leveled);
        int price = PrefixUpgradePrice(_itemSlot.Item, leveled);
        bool atMax = nextType == -1;
        int extraOffset = 0;
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

        float statY = SlotY + 28;
        foreach (string line in GetUpgradeLines(leveled, _itemSlot.Item))
        {
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, line, new Vector2(SlotX + 100, statY), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            statY += 20;
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
        
        Main.hoverItemName = Language.GetTextValue("Mods.ProgressionReforged.PrefixUpgrade.UpgradeHover", leveled.GetLevel() + 1);
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
        int applyPrefix = GetNextUsefulPrefix(_itemSlot.Item, leveled);
        VanillaPrefixTweaker.BypassLevelCheck = true;
        newItem.Prefix(applyPrefix);
        VanillaPrefixTweaker.BypassLevelCheck = false;
        _itemSlot.Item = newItem;
        _itemSlot.Item.position = Main.LocalPlayer.Center;
        _itemSlot.Item.favorited = fav;
        _itemSlot.Item.stack = stack;

        ItemLoader.PostReforge(_itemSlot.Item);
        PopupText.NewText(PopupTextContext.ItemReforge, _itemSlot.Item, _itemSlot.Item.stack, noStack: true);
        SoundEngine.PlaySound(SoundID.Item53);
        SoundEngine.PlaySound(SoundID.Item129);
    }
    
    
    private static IEnumerable<string> GetUpgradeLines(LeveledPrefix current, Item item)
    {
        var lines = new List<string>();

        int nextType = GetNextUsefulPrefix(item, current);
        if (nextType == -1)
            return lines;

        if (PrefixLoader.GetPrefix(nextType) is not LeveledPrefix next)
            return lines;
        
        Item baseItem = new();
        baseItem.SetDefaults(item.type);

        Item nextItem = new();
        nextItem.SetDefaults(item.type);
        VanillaPrefixTweaker.BypassLevelCheck = true;
        nextItem.Prefix(nextType);
        VanillaPrefixTweaker.BypassLevelCheck = false;
        
        lines.Add($"[c/{Color.LightBlue.Hex3()}:{Language.GetTextValue("Mods.ProgressionReforged.PrefixUpgrade.StatsHeader")}]");

        void AddItem(string key, float baseVal, float curVal, float nxtVal, bool inverse = false, bool flat = false)
        {
            int curP;
            int nxtP;
            if (flat)
            {
                curP = (int)MathF.Round(curVal - baseVal);
                nxtP = (int)MathF.Round(nxtVal - baseVal);
            }
            else if (baseVal == 0f)
            {
                curP = (int)MathF.Round(curVal - baseVal);
                nxtP = (int)MathF.Round(nxtVal - baseVal);
            }
            else if (inverse)
            {
                curP = (int)MathF.Round((baseVal - curVal) / baseVal * 100f);
                nxtP = (int)MathF.Round((baseVal - nxtVal) / baseVal * 100f);
            }
            else
            {
                curP = (int)MathF.Round((curVal - baseVal) / baseVal * 100f);
                nxtP = (int)MathF.Round((nxtVal - baseVal) / baseVal * 100f);
            }
            if (curP == nxtP)
                return;

            Color c1 = curP >= 0 ? Color.DarkGreen : Color.DarkRed;
            Color c2 = nxtP >= 0 ? Color.LightGreen : Color.Red;
            string header = Language.GetTextValue($"Mods.ProgressionReforged.PrefixUpgrade.{key}Line");
            
            string curPFormatted = inverse 
                ? $"{curP:+0;-0}".Replace("+", "TEMP").Replace("-", "+").Replace("TEMP", "-") 
                : $"{curP:+0;-0}";
            string nxtPFormatted = inverse 
                ? $"{nxtP:+0;-0}".Replace("+", "TEMP").Replace("-", "+").Replace("TEMP", "-") 
                : $"{nxtP:+0;-0}";
            
            lines.Add($"  [c/{Color.LightBlue.Hex3()}:{header}] [c/{c1.Hex3()}:{curPFormatted}%] [c/FFFFFF:→] [c/{c2.Hex3()}:{nxtPFormatted}%]");
        }

        void AddPrefixStat(string key, float curMult, float nxtMult, bool inverse = false)
        {
            int curP = (int)MathF.Round((inverse ? 1f - curMult : curMult - 1f) * 100f);
            int nxtP = (int)MathF.Round((inverse ? 1f - nxtMult : nxtMult - 1f) * 100f);
            
            if (curP == nxtP)
                return;

            Color c1 = curP >= 0 ? Color.DarkGreen : Color.DarkRed;
            Color c2 = nxtP >= 0 ? Color.LightGreen : Color.Red;
            string header = Language.GetTextValue($"Mods.ProgressionReforged.PrefixUpgrade.{key}Line");
            lines.Add($"  [c/{Color.LightBlue.Hex3()}:{header}] [c/{c1.Hex3()}:{curP:+0;-0}%] [c/FFFFFF:→] [c/{c2.Hex3()}:{nxtP:+0;-0}%]");
        }

        AddItem("Damage", baseItem.damage, item.damage,nextItem.damage);
        AddItem("UseSpeed", baseItem.useTime, item.useTime, nextItem.useTime, true);
        AddPrefixStat("ShootSpeed", current.ShootSpeedMult, next.ShootSpeedMult);
        AddPrefixStat("Size", current.ScaleMult, next.ScaleMult);
        AddPrefixStat("Knockback", current.KnockbackMult, next.KnockbackMult);
        AddItem("ManaCost", baseItem.mana, item.mana, nextItem.mana, true);
        AddItem("CritChance", baseItem.crit, item.crit, nextItem.crit, flat: true);
        
        float curCritDmg = 1f;
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is ICritDamageProvider curProvider)
            curCritDmg = curProvider.CritDamageMult;

        float nxtCritDmg = 1f;
        if (nextItem.prefix > 0 && PrefixLoader.GetPrefix(nextItem.prefix) is ICritDamageProvider nxtProvider)
            nxtCritDmg = nxtProvider.CritDamageMult;

        AddPrefixStat("CritDamage", curCritDmg, nxtCritDmg);
        
        //TODO 1. quitting the game makes the item in the upgrade slot disappear
        //TODO 2. Mana cost rounding is incorrect, and it shows +20% mana cost instead of -20% mana cost for example
        
        return lines;
    }
    
    private static int GetNextUsefulPrefix(Item item, LeveledPrefix prefix)
    {
        int nextType = prefix.GetNext();
        int firstCandidate = nextType;
        
        Item baseItem = new();
        baseItem.SetDefaults(item.type);
        
        while (nextType != -1)
        {
            Item tmp = new();
            tmp.SetDefaults(item.type);
            VanillaPrefixTweaker.BypassLevelCheck = true;
            tmp.Prefix(nextType);
            VanillaPrefixTweaker.BypassLevelCheck = false;
            
            bool diffCurrent = tmp.damage != item.damage || tmp.useTime != item.useTime ||
                               Math.Abs(tmp.shootSpeed - item.shootSpeed) > 0.01 || Math.Abs(tmp.scale - item.scale) > 0.01 ||
                               Math.Abs(tmp.knockBack - item.knockBack) > 0.01 || tmp.mana != item.mana ||
                               tmp.crit != item.crit ||
                               Math.Abs(GetCritDamage(tmp.prefix) - GetCritDamage(item.prefix)) > 0.01;
            
            bool diffBase = tmp.damage != baseItem.damage || tmp.useTime != baseItem.useTime ||
                            Math.Abs(tmp.shootSpeed - baseItem.shootSpeed) > 0.01 || Math.Abs(tmp.scale - baseItem.scale) > 0.01 ||
                            Math.Abs(tmp.knockBack - baseItem.knockBack) > 0.01 || tmp.mana != baseItem.mana ||
                            tmp.crit != baseItem.crit ||
                            Math.Abs(GetCritDamage(tmp.prefix) - GetCritDamage(baseItem.prefix)) > 0.01;

            if (diffCurrent && diffBase)
                break;
            if (PrefixLoader.GetPrefix(nextType) is not LeveledPrefix nextPrefix)
                break;
            nextType = nextPrefix.GetNext();
        }
        return nextType == -1 ? firstCandidate : nextType;
    }

    private static float GetCritDamage(int prefix)
    {
        if (prefix > 0 && PrefixLoader.GetPrefix(prefix) is ICritDamageProvider p)
            return p.CritDamageMult;
        return 1f;
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
            1 => 1.50f,
            2 => 2.00f,
            3 => 3.00f,
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