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

    private PluginConfig _config;
    
    // settings cache
    private bool _hideSpoilers;

    public SettingsTab() {
        this._config = CollectorsAnxietyPlugin.Instance.Configuration;

        this._hideSpoilers = this._config.HideSpoilers;
    }

    public void Draw() {
        if (ImGui.Checkbox(PluginStrings.SettingsTab_SpoilerFreeMode, ref this._hideSpoilers)) {
            this._config.HideSpoilers = this._hideSpoilers;
            this._config.Save();
        }
        ImGuiComponents.HelpMarker(PluginStrings.SettingsTab_SpoilerFreeMode_HelpText);

        if (ImGui.Button(PluginStrings.SettingsTab_ClearHiddenItemList)) {
            this._config.HiddenItems.Clear();
            this._config.Save();
        }

#if DEBUG
        ImGui.Dummy(new Vector2(0, 10));
        ImGui.TextColored(ImGuiColors.DalamudRed, "=== DEV TOOLS ===");
        
        if (ImGui.Button("Pseudo-Localize"))
            UIStrings.Culture = new CultureInfo("qps-ploc");

        ImGui.SameLine();
        if (ImGui.Button("Reset Localization"))
            UIStrings.Culture = new CultureInfo(Injections.PluginInterface.UiLanguage);

#endif


}