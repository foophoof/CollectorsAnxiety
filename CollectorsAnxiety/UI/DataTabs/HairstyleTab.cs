using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class HairstyleTab : BaseTab<HairstyleEntry, CharaMakeCustomize> {
    public HairstyleTab() {
        this.Controller = new HairstyleController();
    }
    
    public override string Name => UIStrings.HairstyleTab_Name;

    protected override void DrawEntryIcons(HairstyleEntry entry) {
        base.DrawEntryIcons(entry);
        
        if (entry.WearableByMale && !entry.WearableByFemale)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Mars, "Limited to male characters.");
        
        if (entry.WearableByFemale && !entry.WearableByMale)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Venus, "Limited to female characters.");
        
        if (entry.WearableByRaceIDs.Contains(7))
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, "Wearable by Hrothgar characters.");
        
        if (entry.WearableByRaceIDs.Contains(8))
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, "Wearable by Viera characters.");

        PlayerState.GetBeastTribeAllowance();
    }
}