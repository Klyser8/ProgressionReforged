using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes.Universal.Damage;

public abstract class DamagePrefix(int level, string chainKey) : LeveledPrefix(level, chainKey)
{
    public override PrefixCategory Category => PrefixCategory.AnyWeapon;
    
}