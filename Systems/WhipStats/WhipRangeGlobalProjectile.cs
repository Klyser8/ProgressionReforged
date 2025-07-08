using ProgressionReforged.Systems.Reforge.Prefixes.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.WhipRange;

public class WhipRangeGlobalProjectile : GlobalProjectile
{
    
    public override bool InstancePerEntity => true;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (!projectile.friendly)
        {
            return;
        }
        if (source is EntitySource_ItemUse itemSource)
        {
            // Check if the item has a whip range prefix
            if (itemSource.Item.prefix > 0 &&
                PrefixLoader.GetPrefix(itemSource.Item.prefix) is IWhipRangeProvider p)
            {
                // Apply the whip range multiplier
                projectile.WhipSettings.RangeMultiplier *= p.WhipRangeMult;
            }
        }
    }
}