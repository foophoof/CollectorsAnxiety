using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class MountTab : DataTab<MountEntry, Mount> {
    public override string Name => "Mounts";

    protected override void DrawEntryIcons(MountEntry entry) {
        base.DrawEntryIcons(entry);

        if (entry.HasActions)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Cog, "Mount has custom actions");

        if (entry.HasUniqueMusic)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Music, "Mount has unique music");

        if (entry.NumberSeats > 1)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Users, $"Mount holds {entry.NumberSeats} players");
    }

    protected override void DrawDevContextMenuItems(MountEntry entry) {
        ImGui.MenuItem($"RideBGM ID: {entry.LuminaEntry.RideBGM.RowId}", false);
    }
}
