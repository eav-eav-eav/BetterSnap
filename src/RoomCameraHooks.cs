using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RebindDevTools;
using UnityEngine;
using Watcher;

namespace BetterSnap;

internal static class RoomCameraHooks
{
    private static bool IsPressingSpace;

    internal static void Apply()
    {
        IL.RoomCamera.GetCameraBestIndex += RoomCamera_GetCameraBestIndex_Modify;
    }

    private static void RoomCamera_GetCameraBestIndex_Modify(ILContext il)
    {
        try
        {
            var c = new ILCursor(il);
            var getWarpInProgressMethod = typeof(WarpPoint)
                .GetMethod($"get_{nameof(WarpPoint.WarpInProgress)}", Utils.StaticFlags);
            // After `WarpPoint.WarpInProgress`
            if (c.TryGotoNext(MoveType.After,
                    i => i.MatchCall(getWarpInProgressMethod)))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate(ShouldStayWithinScreen);
                c.Emit(OpCodes.Or);
            }
        }
        catch (Exception e)
        {
            Main.E(e);
        }

        return;

        static bool ShouldStayWithinScreen(RoomCamera camera, Creature creature)
        {
            var isSBCameraScrollDisabled = ModManager.GetModById("SBCameraScroll")?.enabled != true;
            var teleportSlugcatKey = ModManager.GetModById("rebinddevtools")?.enabled != true
                ? KeyCode.V
                : ModOptions.teleportSlugcat.Value;
            var shouldStayWithinScreen = creature is Player &&
                   camera.game.devToolsActive &&
                   isSBCameraScrollDisabled &&
                   Input.GetKey(teleportSlugcatKey) &&
                   (!Input.GetKey(KeyCode.Space) || IsPressingSpace);
            IsPressingSpace = Input.GetKey(KeyCode.Space);
            return shouldStayWithinScreen;
        }
    }
}