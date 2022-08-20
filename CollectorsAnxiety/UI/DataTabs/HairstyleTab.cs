using System.Linq;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;
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
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Mars, UIStrings.HairstyleTab_Icon_LimitedToMale);
        
        if (entry.WearableByFemale && !entry.WearableByMale)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Venus, UIStrings.HairstyleTab_Icon_LimitedToFemale);

        if (entry.WearableByRaceIDs.Contains(GameCompat.PlayerRace.Hrothgar))
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, UIStrings.HairstyleTab_Icon_AvailableToHrothgar);
        
        if (entry.WearableByRaceIDs.Contains(GameCompat.PlayerRace.Viera))
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, UIStrings.HairstyleTab_Icon_AvailableToViera);

        PlayerState.GetBeastTribeAllowance();
    }

    protected override void DrawDevContextMenuItems(HairstyleEntry entry) {
        var atrList = entry.WearableByRaceIDs.ToList();
        ImGui.MenuItem($"Available To Races:", false);
        foreach (var chunk in atrList.ChunksOf(3)) {
            ImGui.MenuItem("   " + string.Join(", ", chunk), false);
        }
    }
}