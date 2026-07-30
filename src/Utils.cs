using System.Reflection;

namespace BetterSnap;

internal static class Utils
{
    internal const BindingFlags InstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    internal const BindingFlags StaticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    internal static void LogFailedMatching(string hook, int ordinal)
    {
        Main.W($"{hook}_{ordinal} failed to match");
    }
}