using System;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes;

/// 1) The generic reusable scaffolding
public abstract class LeveledPrefix(int level, string chainKey) : ModPrefix
{
   protected readonly int   Level = level;            // -1 … +3
   protected readonly string ChainKey = chainKey;      // Key for the prefix chain, e.g. "Damage", "Defense", "Speed"
   
   // Returns the next leveled prefix in the chain. Override this method to implement the chain logic, returns -1 if there is no next prefix.
   public abstract int GetNext();
   
   // Returns the previous leveled prefix in the chain. Override this method to implement the chain logic, returns -1 if there is no previous prefix.
   public abstract int GetPrevious(); 

   // Base price scaling (override per category if you like)
   public override float RollChance(Item item) => 1f; // keep vanilla chances
   public override void ModifyValue(ref float valueMult) => valueMult *= Level switch
   {
      -1 => 0.75f, // -1: 75% of the base value
       0 => 1.5f,   // 0: 150% of the base value
       1 => 2.0f, // 1: 250% of the base value
       2 => 3.0f,   // 2: 350% of the base value
       3 => 4.0f,   // 3: 500% of the base value
       _ => throw new ArgumentOutOfRangeException(nameof(Level), "Level must be between -1 and +3")
   };
   
   // Level getter
   public int GetLevel() => Level;
}