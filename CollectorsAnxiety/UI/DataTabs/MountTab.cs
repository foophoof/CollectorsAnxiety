using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class MountTab : BaseTab<MountEntry, Mount> {
    public override string Name => UIStrings.MountTab_Name;

    protected override void DrawEntryIcons(MountEntry entry) {
        base.DrawEntryIcons(entry);
        
        if (entry.HasActions)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Cog, "Mount has custom actions");
        
        if (entry.HasUniqueBGM)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Music, "Mount has unique music");
        
        if (entry.NumberSeats > 1)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Users, $"Mount holds {entry.NumberSeats} players");
    }
}