using CollectorsAnxiety.Resources.Localization;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Tabs; 

public class SettingsTab : ITab {
    public string Name => PluginStrings.SettingsTab_Name;
    
    public void Dispose() {
        
    }
    
    public void Draw() {
        ImGui.Text("Settings coming soon!");

        if (ImGui.Button("Unhide All Items")) {
            CollectorsAnxietyPlugin.Instance.Configuration.HiddenItems.Clear();
            CollectorsAnxietyPlugin.Instance.Configuration.Save();
        }
    }


}