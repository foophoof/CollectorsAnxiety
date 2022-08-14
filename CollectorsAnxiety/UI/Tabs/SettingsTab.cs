using System.Globalization;
using System.Numerics;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Resources.Localization;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Tabs; 

public class SettingsTab : ITab {
    public string Name => $"{UIStrings.SettingsTab_Name}###{nameof(SettingsTab)}";

    private readonly PluginConfig _config;
    
    // settings cache
    private bool _hideSpoilers;
    private bool _hiddenItemsInOverview;

    public SettingsTab() {
        this._config = CollectorsAnxietyPlugin.Instance.Configuration;

        this._hideSpoilers = this._config.HideSpoilers;
        this._hiddenItemsInOverview = this._config.CountHiddenItemsInOverview;
    }

    public void Draw() {
        var childSize = ImGui.GetContentRegionAvail();
        var pbs = ImGuiHelpers.GetButtonSize("placeholder");
        var style = ImGui.GetStyle();

        var paddedY = childSize.Y - pbs.Y - 3 * style.ItemSpacing.Y + 2 * style.FramePadding.Y;
        ImGui.BeginChild("SettingsPane", childSize with {Y = paddedY});
        
        ImGui.Text(UIStrings.SettingsTab_Heading_SystemOptions);
        ImGui.Indent();

        if (ImGui.Checkbox(UIStrings.SettingsTab_SpoilerFreeMode, ref this._hideSpoilers)) {
            this._config.HideSpoilers = this._hideSpoilers;
            this._config.Save();
        }

        ImGuiComponents.HelpMarker(UIStrings.SettingsTab_SpoilerFreeMode_HelpText);
        
        if (ImGui.Checkbox(UIStrings.SettingsTab_HiddenItemsInOverview, ref this._hiddenItemsInOverview)) {
            this._config.CountHiddenItemsInOverview = this._hiddenItemsInOverview;
            this._config.Save();
        }

        ImGuiComponents.HelpMarker(UIStrings.SettingsTab_HiddenItemsInOverview_HelpText);
        
        ImGui.Unindent();
        ImGui.Spacing();
        
        ImGui.Text(UIStrings.SettingsTab_Heading_Maintenance);
        ImGui.Indent();

        if (ImGui.Button(UIStrings.SettingsTab_ClearHiddenItemList)) {
            this._config.HiddenItems.Clear();
            this._config.Save();
        }
        
        ImGui.Unindent();

#if DEBUG
        ImGui.Dummy(new Vector2(0, 10));
        ImGui.TextColored(ImGuiColors.DalamudRed, "=== DEV TOOLS ===");
        ImGui.Indent();

        if (ImGui.Button("Pseudo-Localize"))
            UIStrings.Culture = new CultureInfo("qps-ploc");

        ImGui.SameLine();
        if (ImGui.Button("Reset Localization"))
            UIStrings.Culture = new CultureInfo(Injections.PluginInterface.UiLanguage);

        ImGui.Unindent();
#endif

        ImGui.EndChild();
        
        // Footer buttons
        if (ImGui.Button(UIStrings.SettingsTab_Links_Github)) Dalamud.Utility.Util.OpenLink(Constants.GITHUB_URL);
        ImGui.SameLine();
        if (ImGui.Button(UIStrings.SettingsTab_Links_Crowdin)) Dalamud.Utility.Util.OpenLink(Constants.CROWDIN_URL);
    }
}