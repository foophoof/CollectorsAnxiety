using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class ArmoireTab : DataTab<ArmoireEntry, Cabinet> {
    public override string Name => "Armoire";
    public override bool ShowInOverview => false;

    public override void Draw() {
        if (!CollectorsAnxietyPlugin.Instance.GameState.IsArmoirePopulated()) {
            ImGuiHelpers.ScaledDummy(5f);
            using (ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudYellow)) {
                ImGuiUtil.CenteredWrappedText("The Armoire is currently not loaded.");
            }

            ImGuiHelpers.ScaledDummy(10f);

            ImGui.TextWrapped(
                "To refresh information and see collection statistics, please visit an Inn Room and open your Armoire. Collector's Anxiety will automatically refresh this page with the relevant information.");

            return;
        }

        ImGui.TextWrapped(
            "Note: The Armoire tab will only show items stored within the Armoire. Any items in the Glamour Dresser will not be reflected in the below list.");
        base.Draw();
    }

    protected override string GetTagline(ArmoireEntry entry) {
        return entry.Category;
    }
}
