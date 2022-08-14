using System.Numerics;
using CollectorsAnxiety.Resources.Localization;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace CollectorsAnxiety.Util; 

public static class ImGuiUtil {
    public static void CompletionProgressBar(int progress, int total, int height = 20, bool parseColors = false) {
        ImGui.BeginGroup();
        
        var cursor = ImGui.GetCursorPos();
        var sizeVec = new Vector2(ImGui.GetContentRegionAvail().X, height);
        
        var percentage = progress / (float) total;
        var label = string.Format(UIStrings.ImGuiUtil_CompletionProgressBarLabel, percentage, progress, total);
        var labelSize = ImGui.CalcTextSize(label);
        
        if (parseColors) ImGui.PushStyleColor(ImGuiCol.PlotHistogram, GetBarseColor(percentage));
        ImGui.ProgressBar(percentage, sizeVec, "");
        if (parseColors) ImGui.PopStyleColor();
        
        ImGui.SetCursorPos(new Vector2(cursor.X + sizeVec.X - labelSize.X - 4, cursor.Y));
        ImGui.TextUnformatted(label);
        
        ImGui.EndGroup();
    }

    public static Vector4 GetBarseColor(double value) {
        return value switch {
            1 => ImGuiColors.ParsedGold,
            >= 0.95 => ImGuiColors.ParsedOrange,
            >= 0.75 => ImGuiColors.ParsedPurple,
            >= 0.50 => ImGuiColors.ParsedBlue,
            >= 0.25 => ImGuiColors.ParsedGreen,
            _ => ImGuiColors.ParsedGrey * 1.75f,
        };
    }
    
    public static void TextHorizCentered(string text) {
        var availableWidth = ImGui.GetContentRegionAvail().X;
        var textWidth = ImGui.CalcTextSize(text).X;

        // calculate the indentation that centers the text on one line, relative
        // to window left, regardless of the `ImGuiStyleVar_WindowPadding` value
        var textIndentation = (availableWidth - textWidth) * 0.5f;

        // if text is too long to be drawn on one line, `text_indentation` can
        // become too small or even negative, so we check a minimum indentation
        var minIndentation = 20.0f;
        if (textIndentation <= minIndentation) {
            textIndentation = minIndentation;
        }

        ImGui.Dummy(new Vector2(0));
        ImGui.SameLine(textIndentation);
        ImGui.PushTextWrapPos(availableWidth - textIndentation);
        ImGui.TextWrapped(text);
        ImGui.PopTextWrapPos();
    }
    
    public static void HoverMarker(FontAwesomeIcon icon, string helpText)
    {
        ImGui.SameLine();
        ImGui.PushFont(UiBuilder.IconFont);
        ImGui.TextDisabled(icon.ToIconString());
        ImGui.PopFont();
        if (!ImGui.IsItemHovered())
            return;
        ImGui.BeginTooltip();
        ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
        ImGui.TextUnformatted(helpText);
        ImGui.PopTextWrapPos();
        ImGui.EndTooltip();
    }
}