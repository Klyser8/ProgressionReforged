using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace ProgressionReforged.Systems.Hooks;

public class HookStatTweaker : GlobalProjectile
{
    public override void Load()
    {
        // We hook the full Projectile.AI to ensure we see all grappling checks
        // in one pass.
        IL_Projectile.AI_007_GrapplingHooks += PatchHookRanges;
    }

    private void PatchHookRanges(ILContext il)
    {
        var c = new ILCursor(il);

        #region 1) Patch single-type hooks (exact "type == ID" checks)
        // We'll do a while-loop so we can catch multiple checks for the same float
        // or handle all dictionary entries in a single pass. This approach:
        //   - For each "ldc.r4 oldRange -> ble.un -> ldarg.0 -> ldfld type ->
        //     ldc.i4 projectileId -> beq", replace the float with newRange
        //
        //   - Then we move c.Index forward a bit so we don't re-match the same
        //     location over and over, and continue searching until no more matches.
        //
        foreach (var ((oldRange, projId), newRange) in HookStatData.SingleHookRanges)
        {
            // reset c.Index to 0 so each dictionary entry can match from top to bottom
            c.Index = 0;
            while (c.TryGotoNext(
                MoveType.Before,
                i => i.MatchLdcR4(oldRange),
                i => i.MatchBleUn(out _),
                i => i.MatchLdarg(0),
                i => i.MatchLdfld<Projectile>("type"),
                i => i.MatchLdcI4(projId),
                i => i.MatchBeq(out _)
            ))
            {
                // c is now *before* the ldc.r4 oldRange instruction
                // so c.Next is the `ldc.r4 oldRange` instruction.
                c.Next.Operand = newRange;

                // Move forward a few instructions so we can look for subsequent matches.
                // "ldc.r4" -> "ble.un" -> "ldarg.0" -> "ldfld type" -> "ldc.i4" -> "beq"
                // That's 6 instructions total.
                c.Index += 6;
            }
        }
        #endregion

        #region 2) Patch gem hooks (replace the int arithmetic block)
        // The vanilla code is:
        //   int num8 = 300 + (type - 230) * 30;
        //   if (num3 > (float) num8) ...
        // We match all 9 instructions of that arithmetic, remove them, then inject
        // a delegate that picks a new int for [230..235].
        c.Index = 0;
        var localNum8 = -1;
        if (c.TryGotoNext(
            MoveType.Before,
            i => i.MatchLdcI4(300),
            i => i.MatchLdarg(0),
            i => i.MatchLdfld<Projectile>("type"),
            i => i.MatchLdcI4(230),
            i => i.MatchSub(),
            i => i.MatchLdcI4(30),
            i => i.MatchMul(),
            i => i.MatchAdd(),
            i => i.MatchStloc(out localNum8)
        ))
        {
            // remove all 9 instructions
            c.RemoveRange(9);

            // now inject our own code to set that local variable (num8).
            c.Emit(OpCodes.Ldarg_0); // push 'this' (the Projectile)
            c.EmitDelegate<Func<Projectile, int>>(proj => {
                if (HookStatData.GemHookRanges.TryGetValue(proj.type, out int customDist))
                    return customDist;
                // fallback vanilla logic
                return 300 + (proj.type - 230) * 30;
            });
            c.Emit(OpCodes.Stloc, localNum8);
        }
        #endregion

        #region 3) Patch Amber Hook (type=753 => int num9=420)
        // if (this.type == 753) {
        //   int num9 = 420;
        //   if (num3 > (float) num9) ...
        // }
        c.Index = 0;
        if (c.TryGotoNext(
            MoveType.Before,
            i => i.MatchLdarg(0),
            i => i.MatchLdfld<Projectile>("type"),
            i => i.MatchLdcI4(753),
            i => i.MatchBneUn(out _),

            i => i.MatchLdcI4(HookStatData.AmberVanilla),  // = 420
            i => i.MatchStloc(out int localNum9)
        ))
        {
            // The cursor is at the first instruction in that sequence (ldarg(0)).
            // We want to land on "ldc.i4 420". That is instruction #4 from the start.
            // Actually, indices: 0=ldarg(0),1=ldfld,2=ldc.i4(753),3=bne.un,4=ldc.i4(420),5=stloc
            c.Index += 4; // now c.Next is the "ldc.i4(420)" instruction
            c.Next.Operand = HookStatData.AmberNew; // 368
        }
        #endregion

        #region 4) Patch multi-type block for [486..489]
        // Vanilla: if (num3 > 480f && this.type >= 486 && this.type <= 489) { ... }
        //
        // (a) If you just want them ALL the same range, do something like:
        //     "replace 480f with 384f" to give them 24 tiles. That's easy:
        //         ldloc.s num3
        //         ldc.r4 480
        //         ble.un.s ...
        //         ldarg.0 -> ldfld type
        //         ldc.i4 486
        //         blt ...
        //         ...
        // (b) If each one needs a DIFFERENT range, you must remove or patch the entire
        //     block, then do a small injection that picks the correct distance from
        //     a dictionary. Example:

        float fallback480 = 480f;
        c.Index = 0;
        if (c.TryGotoNext(
            MoveType.Before,
            i => i.MatchLdcR4(480f),    // if (num3 > 480f)
            i => i.MatchBleUn(out _),  // ...
            i => i.MatchLdarg(0),
            i => i.MatchLdfld<Projectile>("type"),
            i => i.MatchLdcI4(486),    // if (this.type < 486) => skip
            i => i.MatchBlt(out _),
            i => i.MatchLdarg(0),
            i => i.MatchLdfld<Projectile>("type"),
            i => i.MatchLdcI4(489),
            i => i.MatchBgt(out _)
        ))
        {
            // We found the block that lumps 486..489 under "num3 > 480f".
            // If you want them all the same distance, you can just do:
            //    c.Next.Operand = 384f; // i.e. 24f*16f
            // Done. But let's see the "per-type" approach:

            // We'll remove the `ldc.r4 480f` and the subsequent "ble.un"
            // then inject code to do a custom comparison. However, a simpler approach
            // is to just "replace 480f with 9999f" or something, then do a separate check,
            // but let's do a delegate injection.

            // 1) Store the index of 'ldc.r4 480f'
            int startIndex = c.Index;
            // We matched 10 instructions in this block. Let's remove them all,
            // then re-inject a custom "if (num3 > someDistance) ... " block.
            // But that means we also have to replicate the "type >= 486 && type <= 489" check.
            // We'll show a minimal injection:

            for (int n = 0; n < 10; n++) {
                c.Remove();
            }

            // Now insert custom logic:
            // Pseudocode:
            //   if (CheckMultiHookRange(num3, this))
            //       this.ai[0] = 1f;
            //
            // We rely on a helper method that returns "true" if the hook is out of range.
            // That method can do "if (type in [486..489]) use a dictionary, else do fallback."

            c.Emit(OpCodes.Ldarg_0);    // push 'this' (the Projectile) so we know 'type'
            c.Emit(OpCodes.Ldloc, 4);  // usually local #4 is 'num3', but this might differ in your build
                                       // you must confirm which local index is `num3` in your IL. 
                                       // If it's not local #4, adjust accordingly!

            // Alternatively, if you're unsure of num3's local index, you can search the IL to see
            // which stloc N is used right after "conv.r4" from some distance calculation. 
            // For brevity, let's assume it's local #4.

            c.EmitDelegate<Func<Projectile, float, bool>>((proj, distance) => {
                // If type is in [486..489], pick from our dictionary:
                if (proj.type >= 486 && proj.type <= 489) {
                    float required = HookStatData.MultiHookRanges.TryGetValue(proj.type, out float customDist)
                                     ? customDist
                                     : 480f; // fallback
                    return distance > required;
                }
                // otherwise fallback to the vanilla check (or always false).
                return false;
            });

            // Now we have a bool on stack: 'true' => out of range. 
            // We do: if (true) => this.ai[0] = 1f; else continue.
            // So we create instructions:
            var labelContinue = c.DefineLabel();
            c.Emit(OpCodes.Brfalse_S, labelContinue); // if false => jump to labelContinue

            // this.ai[0] = 1f
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(Projectile).GetField("ai")); // load array
            c.Emit(OpCodes.Ldc_I4_0);                                 // index 0
            c.Emit(OpCodes.Ldc_R4, 1f);                               // float 1.0
            c.Emit(OpCodes.Stelem_R4);                                // store into ai[0]

            c.MarkLabel(labelContinue);
            // done injecting
        }

        #endregion
    }
}
