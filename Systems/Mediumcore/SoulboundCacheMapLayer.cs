using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProgressionReforged.Systems.MediumcoreDeath;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace ProgressionReforged.Systems.Mediumcore;

public class SoulboundCacheMapLayer : ModMapLayer
{
    private Asset<Texture2D>? _iconTexture;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        _iconTexture = ModContent.Request<Texture2D>("ProgressionReforged/Content/UI/SoulboundCache");
    }

    public override void Unload()
    {
        _iconTexture = null;
    }

    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    {
        if (_iconTexture is not { IsLoaded: true } icon)
            return;

        MediumcoreDropSystem? system = MediumcoreDropSystem.Instance;
        if (system == null)
            return;

        var drops = system.StoredDrops;
        if (drops.Count == 0)
            return;

        foreach (TagCompound drop in drops)
        {
            if (!drop.ContainsKey("pos"))
                continue;

            Vector2 worldPosition = drop.Get<Vector2>("pos");
            Vector2 mapPosition = worldPosition / 16f;

            bool arrived = drop.ContainsKey("arrived") && drop.GetBool("arrived");
            string owner = drop.ContainsKey("owner") ? drop.GetString("owner") : string.Empty;

            // Color color = arrived ? Color.White : Color.White * 0.6f;
            // if (!string.IsNullOrEmpty(owner) && Main.LocalPlayer.name == owner)
                // color = arrived ? Color.LimeGreen : Color.Goldenrod;

            MapOverlayDrawContext.DrawResult drawResult = context.Draw(icon.Value, mapPosition, Alignment.Center);
            if (drawResult.IsMouseOver)
            {
                string cacheName = Language.GetTextValue("Mods.ProgressionReforged.Projectiles.SoulboundCache.DisplayName");
                if (string.IsNullOrWhiteSpace(cacheName))
                    cacheName = "Soulbound Cache";
                string ownerText = string.IsNullOrWhiteSpace(owner) ? cacheName : $"{owner}'s {cacheName}";

                if (!arrived)
                {
                    string travelingText = Language.GetTextValue("Mods.ProgressionReforged.Mediumcore.SoulboundCacheTraveling");
                    ownerText += string.IsNullOrWhiteSpace(travelingText) ? " (Traveling)" : $" ({travelingText})";
                }

                text = ownerText;
            }
        }
    }
}