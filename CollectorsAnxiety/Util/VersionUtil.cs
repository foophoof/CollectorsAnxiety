using System;
using System.Reflection;
using System.Security.Cryptography;

namespace CollectorsAnxiety.Util; 

public static class VersionUtil {
    public static string GetCurrentMajMinBuild() {
        return Assembly.GetExecutingAssembly().GetName().Version!.GetMajMinBuild();
    }
    
    public static string GetMajMinBuild(this Version version) {
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }

    public static dynamic? AddThings(params dynamic?[] args) {
        dynamic sum = new System.Dynamic.ExpandoObject();
        
        foreach (var arg in args) {
            sum += arg;
        }

        return sum;
    }
}