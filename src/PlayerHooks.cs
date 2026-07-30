using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace BetterSnap;

internal static class PlayerHooks
{
    internal static void Apply()
    {
        IL.Player.Update += Player_Update_Modify;
    }

    private static void Player_Update_Modify(ILContext il)
    {
        try
        {
            var c = new ILCursor(il);
            var resetRopeLengthMethod = typeof(Player.Tongue)
                .GetMethod(nameof(Player.Tongue.resetRopeLength), Utils.InstanceFlags);
            // After `this.tongue.resetRopeLength();`
            if (c.TryGotoNext(MoveType.After,
                    i => i.MatchCallvirt(resetRopeLengthMethod)))
            {
                // Before `2`
                if (c.TryGotoNext(MoveType.Before,
                        i => i.MatchLdcR4(12f)))
                {
                    c.Remove();
                    c.Emit(OpCodes.Ldc_R4, 1f);
                }
                else
                {
                    Utils.LogFailedMatching(nameof(Player_Update_Modify), 2);
                }
            }
            else
            {
                Utils.LogFailedMatching(nameof(Player_Update_Modify), 1);
            }
        }
        catch (Exception e)
        {
            Main.E(e);
        }
    }
}