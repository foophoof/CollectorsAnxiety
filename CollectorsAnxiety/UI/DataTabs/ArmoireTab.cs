using System.Numerics;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class ArmoireTab : BaseTab<ArmoireEntry, Cabinet> {
    public override string Name => UIStrings.ArmoireTab_Name;
    public override bool ShowInOverview => false;

    public override void Draw() {
        if (!CollectorsAnxietyPlugin.Instance.GameState.IsArmoirePopulated()) {
            ImGui.Dummy(new Vector2(0, 20));
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
            ImGuiUtil.TextHorizCentered(UIStrings.ArmoireTab_ArmoireNotLoadedError_Title);
            ImGuiUtil.TextHorizCentered(UIStrings.ArmoireTab_ArmoireNotLoadedError_Message);
            ImGui.PopStyleColor();
            return;
        }
        
        ImGui.TextWrapped(UIStrings.ArmoireTab_ArmoireNote);
        base.Draw();
    }

    protected override string GetTagline(ArmoireEntry entry) => entry.Category;
}