using System;
using System.Reflection;

namespace CollectorsAnxiety.Util;

public static class VersionUtil {
    private static readonly Version PluginVersion = Assembly.GetExecutingAssembly().GetName().Version!;

    public static string GetCurrentMajMinBuild() {
        return PluginVersion.GetMajMinBuild();
    }

    public static string GetMajMinBuild(this Version version) {
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }
}
