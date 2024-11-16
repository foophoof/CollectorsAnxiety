using System.Globalization;
using Dalamud.Game.Text.SeStringHandling;
using Lumina.Text.ReadOnly;

namespace CollectorsAnxiety.Util;

public static class StringUtil {
    public static string ToTitleCase(this string str) {
        return ToTitleCase(str, CultureInfo.CurrentCulture);
    }

    public static string ToTitleCase(this SeString seString) {
        return ToTitleCase(seString.ToString());
    }

    public static string ToTitleCase(this ReadOnlySeString seString) {
        return ToTitleCase(seString.ExtractText());
    }

    public static string ToTitleCase(this string str, CultureInfo culture) {
        var textInfo = culture.TextInfo;
        return textInfo.ToTitleCase(str);
    }
}
