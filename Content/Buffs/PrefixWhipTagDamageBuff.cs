using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProgressionReforged.Content.Buffs;

public class PrefixWhipTagDamageBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        // Mark this as a tag buff so minions recognize it
        BuffID.Sets.IsATagBuff[Type] = true;
    }
}