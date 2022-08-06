using System;
using System.Reflection;

namespace CollectorsAnxiety.Util; 

public static class VersionUtil {
    public static string GetCurrentMajMinBuild() {
        return Assembly.GetExecutingAssembly().GetName().Version!.GetMajMinBuild();
    }
    
    public static string GetMajMinBuild(this Version version) {
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }
}