using System.Globalization;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Resources.Localization;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Tabs; 

public class SettingsTab : ITab {
    public string Name => $"{PluginStrings.SettingsTab_Name}###{nameof(SettingsTab)}";

    public void Draw() {
        ImGui.Text("Settings coming soon!");

        if (ImGui.Button("Unhide All Items")) {
            CollectorsAnxietyPlugin.Instance.Configuration.HiddenItems.Clear();
            CollectorsAnxietyPlugin.Instance.Configuration.Save();
        }
        
#if DEBUG
        ImGui.TextColored(ImGuiColors.DalamudRed, "=== DEV TOOLS ===");
        
        if (ImGui.Button("Pseudo-Localize"))
            PluginStrings.Culture = new CultureInfo("qps-ploc");

        ImGui.SameLine();
        if (ImGui.Button("Reset Localization"))
            PluginStrings.Culture = new CultureInfo(Injections.PluginInterface.UiLanguage);

#endif
    }


}