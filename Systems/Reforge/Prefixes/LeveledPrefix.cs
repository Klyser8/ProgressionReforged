using System;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Reforge.Prefixes;

/// 1) The generic reusable scaffolding
public abstract class LeveledPrefix(int level, string chainKey) : ModPrefix
{
   protected readonly int   Level = level;            // -1 … +3
   protected readonly string ChainKey = chainKey;      // Key for the prefix chain, e.g. "Damage", "Defense", "Speed"

   public float DamageMult { get; private set; } = 1.00f;
   public float KnockbackMult { get; private set; } = 1.00f;
   public float UseTimeMult { get; private set; } = 1.00f;
   public float ScaleMult { get; private set; } = 1.00f;
   public float ShootSpeedMult { get; private set; } = 1.00f;
   public float ManaMult { get; private set; } = 1.00f;
   public int CritBonus { get; private set; } = 0;
   // This needs to be assigned manually in subclasses, in order to calculate the prefixed item's value. 
   // Not to be confused with ICritDamageProvider#CritDamageMult, which is used to provide the crit damage multiplier to the crit damage system.
   public float CritDamageMultInternal { get; protected set; } = 1.00f;
   
   // This method MUST be called in any subclass. The assignment of the stats to the class fields is done here, and is needed to calculate the prefixed item's value.
   public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult,
      ref float shootSpeedMult, ref float manaMult, ref int critBonus)
   {
      // Assign the stats from this method to the class fields
      DamageMult = damageMult;
      KnockbackMult = knockbackMult;
      UseTimeMult = useTimeMult;
      ScaleMult = scaleMult;
      ShootSpeedMult = shootSpeedMult;
      ManaMult = manaMult;
      CritBonus = critBonus;
   }
   
   // Returns the next leveled prefix in the chain. Override this method to implement the chain logic, returns -1 if there is no next prefix.
   public abstract int GetNext();
   
   // Returns the previous leveled prefix in the chain. Override this method to implement the chain logic, returns -1 if there is no previous prefix.
   public abstract int GetPrevious(); 

   // Base price scaling (override per category if you like)
   // public override void ModifyValue(ref float valueMult) {
   //    // 1) Convert *all* prefix stats into signed percent deltas
   //    // float delta =
   //    //    WeightedDelta(DamageMult, PriceWeight.Damage)
   //    //    + WeightedDelta(UseTimeMult, PriceWeight.UseSpeed, inverse: true)
   //    //    + WeightedDelta(ShootSpeedMult, PriceWeight.ShootSpeed)
   //    //    + WeightedDelta(ScaleMult, PriceWeight.Size)
   //    //    + WeightedDelta(KnockbackMult, PriceWeight.Knockback)
   //    //    + WeightedDelta(ManaMult, PriceWeight.ManaCost, inverse: true)
   //    //    + CritBonus / 100f * PriceWeight.CritChance
   //    //    + (CritDamageMultInternal - 1f) * PriceWeight.CritDamage;
   //    //
   //    // // stack on top of anything another mod might have done
   //    // valueMult *= 1f + delta;
   //    //
   //    // // tier price ladder
   //    // valueMult *= Level switch
   //    // {
   //    //    -1 => .75f, 
   //    //    0 => 1.5f, 
   //    //    1 => 2f, 
   //    //    2 => 3f, 
   //    //    3 => 4f, 
   //    //    _ => 1f
   //    // };
   //    valueMult = Level;
   // }

   // Level getter
   public int GetLevel() => Level;
}