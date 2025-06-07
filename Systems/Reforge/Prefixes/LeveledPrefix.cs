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
   public override void ModifyValue(ref float valueMult) {
      valueMult *= Level switch {
         -1 => 0.90f,   // 10 % cheaper than vanilla
         0 => 1.00f,
         1 => 1.20f,   // 20 % dearer
         2 => 1.50f,
         3 => 2.00f,
         _ => 1f
      };
   }
   
   // Level getter
   public int GetLevel() => Level;
}