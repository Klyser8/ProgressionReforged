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
using ProgressionReforged.Systems.Reforge.Prefixes.Accessories;
using ProgressionReforged.Systems.Reforge.Prefixes.Summon;
using ProgressionReforged.Systems.Reforge.Prefixes.Universal.CritDamage;
using ReLogic.Content;
using Terraria.GameContent.Creative;

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
        int skippedLevels = 0;
        if (nextType != -1 && PrefixLoader.GetPrefix(nextType) is LeveledPrefix nextPrefix)
            skippedLevels = Math.Max(0, nextPrefix.GetLevel() - leveled.GetLevel() - 1);

        int price = PrefixUpgradePrice(_itemSlot.Item, leveled, skippedLevels);
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


        void AddItem(string key, float baseVal, float curVal, float nxtVal, bool inverse = false, bool flipSign = false, bool flat = false, bool showPercent = true)
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
            
            string curPFormatted = flipSign
                ? $"{curP:+0;-0}".Replace("+", "TEMP").Replace("-", "+").Replace("TEMP", "-")
                : $"{curP:+0;-0}";
            string nxtPFormatted = flipSign
                ? $"{nxtP:+0;-0}".Replace("+", "TEMP").Replace("-", "+").Replace("TEMP", "-")
                : $"{nxtP:+0;-0}";
            
            
            string suffix = showPercent ? "%" : string.Empty;
            lines.Add($"  [c/{Color.LightBlue.Hex3()}:{header}] [c/{c1.Hex3()}:{curPFormatted}{suffix}] [c/FFFFFF:→] [c/{c2.Hex3()}:{nxtPFormatted}{suffix}]");
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
        AddItem("UseSpeed", baseItem.useTime, item.useTime, nextItem.useTime, inverse: true, flipSign: false);
        AddPrefixStat("ShootSpeed", current.ShootSpeedMult, next.ShootSpeedMult);
        AddPrefixStat("Size", current.ScaleMult, next.ScaleMult);
        AddPrefixStat("Knockback", current.KnockbackMult, next.KnockbackMult);
        AddItem("ManaCost", baseItem.mana, item.mana, nextItem.mana, inverse: true, flipSign: true);
        AddItem("CritChance", baseItem.crit, item.crit, nextItem.crit, flat: true);
        
        float curCritDmg = 1f;
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is ICritDamageProvider critDamageProvider)
            curCritDmg = critDamageProvider.CritDamageMult;
        float nxtCritDmg = 1f;
        if (nextItem.prefix > 0 && PrefixLoader.GetPrefix(nextItem.prefix) is ICritDamageProvider nxtProvider)
            nxtCritDmg = nxtProvider.CritDamageMult;
        AddPrefixStat("CritDamage", curCritDmg, nxtCritDmg);
        
        float currentWhipRange = 1f;
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is IWhipRangeProvider whipRangeProvider)
            currentWhipRange = whipRangeProvider.WhipRangeMult;
        float nextWhipRange = 1f;
        if (nextItem.prefix > 0 && PrefixLoader.GetPrefix(nextItem.prefix) is IWhipRangeProvider nextWhipRangeProvider)
            nextWhipRange = nextWhipRangeProvider.WhipRangeMult;
        AddPrefixStat("WhipRange", currentWhipRange, nextWhipRange);
        
        float currentTagDamage = 1f;
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is IWhipTagDamageProvider tagProvider)
            currentTagDamage = tagProvider.WhipTagDamageMult;
        float nextTagDamage = 1f;
        if (nextItem.prefix > 0 && PrefixLoader.GetPrefix(nextItem.prefix) is IWhipTagDamageProvider nextTagProvider)
            nextTagDamage = nextTagProvider.WhipTagDamageMult;
        AddPrefixStat("WhipTagDamage", currentTagDamage, nextTagDamage);
        
        
        if (item.prefix > 0 && PrefixLoader.GetPrefix(item.prefix) is IAccessoryPrefixProvider accCur)
        {
            IAccessoryPrefixProvider? accNext = null;
            if (nextItem.prefix > 0 && PrefixLoader.GetPrefix(nextItem.prefix) is IAccessoryPrefixProvider n)
                accNext = n;

            AddItem("Defense", 0, accCur.DefenseBonus, accNext?.DefenseBonus ?? 0, flat: true, showPercent: false);
            AddItem("Health", 0, accCur.HealthBonus, accNext?.HealthBonus ?? 0, flat: true, showPercent: false);
            AddItem("AccessoryCrit", 0, accCur.CritBonus, accNext?.CritBonus ?? 0, flat: true);
            AddItem("ArmorPen", 0, accCur.ArmorPenBonus, accNext?.ArmorPenBonus ?? 0, flat: true, showPercent: false);

            AddPrefixStat("AccessoryDamage", accCur.DamageMult, accNext?.DamageMult ?? 1f);
            AddPrefixStat("ManaRegen", accCur.ManaRegenMult, accNext?.ManaRegenMult ?? 1f);
            AddPrefixStat("MovementSpeed", accCur.MovementSpeedMult, accNext?.MovementSpeedMult ?? 1f);
            AddPrefixStat("JumpHeight", accCur.JumpHeightMult, accNext?.JumpHeightMult ?? 1f);
            AddPrefixStat("KnockbackResist", accCur.KnockbackMult, accNext?.KnockbackMult ?? 1f);
            AddPrefixStat("AccessoryCritDamage", accCur.CritDamageMult, accNext?.CritDamageMult ?? 1f);
        }
        
        return lines;
    }
    
    private static int GetNextUsefulPrefix(Item item, LeveledPrefix prefix)
    {
        int nextType = prefix.GetNext();
        
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
                               Math.Abs(tmp.shootSpeed - item.shootSpeed) > 0.001 || Math.Abs(tmp.scale - item.scale) > 0.001 ||
                               Math.Abs(tmp.knockBack - item.knockBack) > 0.001 || tmp.mana != item.mana ||
                               tmp.crit != item.crit ||
                               Math.Abs(GetCritDamage(tmp.prefix) - GetCritDamage(item.prefix)) > 0.001 ||
                               Math.Abs(GetWhipRange(tmp.prefix) - GetWhipRange(item.prefix)) > 0.001 ||
                               Math.Abs(GetWhipTagDamageMult(tmp.prefix) - GetWhipTagDamageMult(item.prefix)) > 0.001;
            
            bool diffBase = tmp.damage != baseItem.damage || tmp.useTime != baseItem.useTime ||
                            Math.Abs(tmp.shootSpeed - baseItem.shootSpeed) > 0.001 || Math.Abs(tmp.scale - baseItem.scale) > 0.001 ||
                            Math.Abs(tmp.knockBack - baseItem.knockBack) > 0.001 || tmp.mana != baseItem.mana ||
                            tmp.crit != baseItem.crit ||
                            Math.Abs(GetWhipRange(tmp.prefix) - GetWhipRange(baseItem.prefix)) > 0.001 ||
                            Math.Abs(GetWhipTagDamageMult(tmp.prefix) - GetWhipTagDamageMult(baseItem.prefix)) > 0.001;

            if (prefix is IAccessoryPrefixProvider accCur && PrefixLoader.GetPrefix(tmp.prefix) is IAccessoryPrefixProvider accTmp)
            {
                diffCurrent |= accCur.DefenseBonus != accTmp.DefenseBonus ||
                               accCur.HealthBonus != accTmp.HealthBonus ||
                               accCur.CritBonus != accTmp.CritBonus ||
                               accCur.ArmorPenBonus != accTmp.ArmorPenBonus ||
                               Math.Abs(accCur.CritDamageMult - accTmp.CritDamageMult) > 0.001f ||
                               Math.Abs(accCur.JumpHeightMult - accTmp.JumpHeightMult) > 0.001f ||
                               Math.Abs(accCur.KnockbackMult - accTmp.KnockbackMult) > 0.001f ||
                               Math.Abs(accCur.DamageMult - accTmp.DamageMult) > 0.001f ||
                               Math.Abs(accCur.ManaRegenMult - accTmp.ManaRegenMult) > 0.001f ||
                               Math.Abs(accCur.MovementSpeedMult - accTmp.MovementSpeedMult) > 0.001f;

                diffBase |= accTmp.DefenseBonus != 0 || accTmp.HealthBonus != 0 || accTmp.CritBonus != 0 || accTmp.ArmorPenBonus != 0 ||
                            Math.Abs(accTmp.CritDamageMult - 1f) > 0.01f || Math.Abs(accTmp.JumpHeightMult - 1f) > 0.001f ||
                            Math.Abs(accTmp.KnockbackMult - 1f) > 0.01f || Math.Abs(accTmp.DamageMult - 1f) > 0.001f ||
                            Math.Abs(accTmp.ManaRegenMult - 1f) > 0.01f || Math.Abs(accTmp.MovementSpeedMult - 1f) > 0.001f;
            }
            
            if (diffCurrent && diffBase)
                return nextType;
            if (PrefixLoader.GetPrefix(nextType) is not LeveledPrefix nextPrefix)
                break;
            nextType = nextPrefix.GetNext();
        }
        return -1;
    }

    private static float GetCritDamage(int prefix)
    {
        if (prefix > 0 && PrefixLoader.GetPrefix(prefix) is ICritDamageProvider p)
            return p.CritDamageMult;
        return 1f;
    }
    
    private static float GetWhipRange(int prefix)
    {
        if (prefix > 0 && PrefixLoader.GetPrefix(prefix) is IWhipRangeProvider p)
            return p.WhipRangeMult;
        return 1f;
    }
    
    private static float GetWhipTagDamageMult(int prefix)
    {
        if (prefix > 0 && PrefixLoader.GetPrefix(prefix) is IWhipTagDamageProvider p)
            return p.WhipTagDamageMult;
        return 1f;
    }

    private static int PrefixUpgradePrice(Item item, LeveledPrefix leveled, int skippedLevels)
    {
        int baseValue = ContentSamples.ItemsByType[item.type].value;
        float levelMult = leveled.GetLevel() switch
        {
            -1 => 0.75f,
            0 => 1.50f, // + 0.75
            1 => 2.66f, // + 1.16f
            2 => 4.20f, // + 1.54f
            3 => 6.00f, // + 2.00f
            _ => 1.00f
        } + skippedLevels * 2f;

        if (leveled is AccessoryPrefix acc)
        {
            float weight = AccessoryPriceConfig.GetWeight(acc);
            return (int)(baseValue * weight * levelMult);
        }
        float delta =
            PriceHelper.WeightedDelta(leveled.DamageMult, PriceHelper.PriceWeight.Damage) +
            PriceHelper.WeightedDelta(leveled.UseTimeMult, PriceHelper.PriceWeight.UseSpeed, true) +
            PriceHelper.WeightedDelta(leveled.ShootSpeedMult, PriceHelper.PriceWeight.ShootSpeed) +
            PriceHelper.WeightedDelta(leveled.ScaleMult, PriceHelper.PriceWeight.Size) +
            PriceHelper.WeightedDelta(leveled.KnockbackMult, PriceHelper.PriceWeight.Knockback) +
            PriceHelper.WeightedDelta(leveled.ManaMult, PriceHelper.PriceWeight.ManaCost, true) +
            leveled.CritBonus / 100f * PriceHelper.PriceWeight.CritChance +
            (leveled.CritDamageMultInternal - 1f) * PriceHelper.PriceWeight.CritDamage +
            (leveled.WhipRangeMultInternal - 1f) * PriceHelper.PriceWeight.WhipRange +
            (leveled.WhipTagDamageMultInternal - 1f) * PriceHelper.PriceWeight.WhipTagDamage;
        return (int)(baseValue * (1f + delta) * levelMult);
    }
}