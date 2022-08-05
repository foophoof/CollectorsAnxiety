using System.Numerics;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class ArmoireTab : BaseTab<ArmoireEntry, Cabinet> {
    public override string Name => PluginStrings.ArmoireTab_Name;
    public override bool ShowInOverview => false;

    public override void Draw() {
        if (!CollectorsAnxietyPlugin.Instance.GameState.IsArmoirePopulated()) {
            ImGui.Dummy(new Vector2(0, 20));
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
            ImGuiUtil.TextHorizCentered(PluginStrings.ArmoireTab_ArmoireNotLoadedError_Title);
            ImGuiUtil.TextHorizCentered(PluginStrings.ArmoireTab_ArmoireNotLoadedError_Message);
            ImGui.PopStyleColor();
            return;
        }
        
        ImGui.TextWrapped(PluginStrings.ArmoireTab_ArmoireNote);
        base.Draw();
    }
}