using RebindDevTools;
using UnityEngine;

namespace BetterSnap;

internal static class RebindDevToolsAccessor
{
    internal static KeyCode GetTeleportSlugcatKey()
    {
        return ModOptions.teleportSlugcat.Value;
    }
}