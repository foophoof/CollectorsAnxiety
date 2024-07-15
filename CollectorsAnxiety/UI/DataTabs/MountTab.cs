using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class MountTab : DataTab<MountEntry, Mount> {
    public override string Name => UIStrings.MountTab_Name;

    protected override void DrawEntryIcons(MountEntry entry) {
        base.DrawEntryIcons(entry);

        if (entry.HasActions)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Cog, UIStrings.MountTab_HasCustomActions);

        if (entry.HasUniqueMusic)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Music, UIStrings.MountTab_HasUniqueBGM);

        if (entry.NumberSeats > 1)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Users, string.Format(UIStrings.MountTab_HasExtraSeats, entry.NumberSeats));
    }

    protected override void DrawDevContextMenuItems(MountEntry entry) {
        ImGui.MenuItem($"RideBGM ID: {entry.LuminaEntry.RideBGM.Row}", false);
    }
}
