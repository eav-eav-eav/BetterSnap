using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;
using static BetterSnap.Metadata;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace BetterSnap;

[BepInPlugin(ID, Name, Version)]
[BepInDependency("rebinddevtools", BepInDependency.DependencyFlags.SoftDependency)]
internal sealed class Main : BaseUnityPlugin
{
    private static ManualLogSource? _logger;

    private void OnEnable()
    {
        var logger = this.Logger;
        _logger = logger;
        logger.LogInfo($"Loading {Name}...");
        PlayerHooks.Apply();
        RoomCameraHooks.Apply();
        logger.LogInfo($"Loaded {Name}!");
    }

    private void OnDisable()
    {
        _logger = null;
    }

    internal static void D(object msg)
    {
        _logger?.LogDebug(msg);
    }

    internal static void I(object msg)
    {
        _logger?.LogInfo(msg);
    }

    internal static void W(object msg)
    {
        _logger?.LogWarning(msg);
    }

    internal static void E(object msg)
    {
        _logger?.LogError(msg);
    }
}