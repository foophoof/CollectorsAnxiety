using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class ArmoireTab : DataTab<ArmoireEntry, Cabinet> {
    public override string Name => UIStrings.ArmoireTab_Name;
    public override bool ShowInOverview => false;

    public override void Draw() {
        if (!CollectorsAnxietyPlugin.Instance.GameState.IsArmoirePopulated()) {
            ImGuiHelpers.ScaledDummy(5f);
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
            ImGuiUtil.CenteredWrappedText(UIStrings.ArmoireTab_ArmoireNotLoadedError_Title);
            ImGui.PopStyleColor();

            ImGuiHelpers.ScaledDummy(10f);

            ImGui.TextWrapped(UIStrings.ArmoireTab_ArmoireNotLoadedError_Message);

            return;
        }

        ImGui.TextWrapped(UIStrings.ArmoireTab_ArmoireNote);
        base.Draw();
    }

    protected override string GetTagline(ArmoireEntry entry) {
        return entry.Category;
    }
}
