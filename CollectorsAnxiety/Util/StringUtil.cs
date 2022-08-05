using System.Globalization;
using CollectorsAnxiety.Resources.Localization;
using Dalamud.Game.Text.SeStringHandling;

namespace CollectorsAnxiety.Util;

public static class StringUtil {
    public static string ToTitleCase(this string str) {
        return ToTitleCase(str, PluginStrings.Culture);
    }

    public static string ToTitleCase(this SeString seString) {
        return ToTitleCase(seString.ToString());
    }

    public static string ToTitleCase(this string str, CultureInfo culture) {
        var textInfo = culture.TextInfo;
        return textInfo.ToTitleCase(str);
    }
}