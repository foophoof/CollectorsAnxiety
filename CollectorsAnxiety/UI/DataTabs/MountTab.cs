using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class MountTab : BaseTab<MountEntry, Mount> {
    protected override TableColumn[] ExtraColumns { get; } = {
        new("Seats", ImGuiTableColumnFlags.WidthFixed, 48),
    };

    public override string Name => PluginStrings.MountTab_Name;
    
    protected override void DrawExtraColumns(MountEntry entry) {
        ImGui.TableSetColumnIndex(4);
        ImGui.Text($"x{entry.NumberSeats}");
    }
}