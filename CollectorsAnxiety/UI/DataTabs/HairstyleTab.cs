using System.Linq;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using ImGuiNET;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class HairstyleTab : DataTab<HairstyleEntry, CharaMakeCustomize> {
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

        var maleHrothgar = entry.WearableByMaleRaceIDs.Contains(GameCompat.PlayerRace.Hrothgar);
        var femaleHrothgar = entry.WearableByFemaleRaceIDs.Contains(GameCompat.PlayerRace.Hrothgar);
        var maleViera = entry.WearableByMaleRaceIDs.Contains(GameCompat.PlayerRace.Viera);
        var femaleViera = entry.WearableByFemaleRaceIDs.Contains(GameCompat.PlayerRace.Viera);

        if (maleHrothgar && femaleHrothgar) {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, UIStrings.HairstyleTab_Icon_AvailableToHrothgar);
        } else if (maleHrothgar && !femaleHrothgar) {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, UIStrings.HairstyleTab_Icon_AvailableToMaleHrothgar);
        } else if (!maleHrothgar && femaleHrothgar) {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, UIStrings.HairstyleTab_Icon_AvailableToFemaleHrothgar);
        }

        if (maleViera && femaleViera) {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, UIStrings.HairstyleTab_Icon_AvailableToViera);
        } else if (maleViera && !femaleViera) {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, UIStrings.HairstyleTab_Icon_AvailableToMaleViera);
        } else if (!maleViera && femaleViera) {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, UIStrings.HairstyleTab_Icon_AvailableToFemaleViera);
        }
    }

    protected override void DrawDevContextMenuItems(HairstyleEntry entry) {
        var matrList = entry.WearableByMaleRaceIDs.ToList();
        ImGui.MenuItem("Available To Races (Male):", false);
        foreach (var chunk in matrList.ChunksOf(3)) {
            ImGui.MenuItem("   " + string.Join(", ", chunk), false);
        }

        var fatrList = entry.WearableByFemaleRaceIDs.ToList();
        ImGui.MenuItem("Available To Races (Female):", false);
        foreach (var chunk in fatrList.ChunksOf(3)) {
            ImGui.MenuItem("   " + string.Join(", ", chunk), false);
        }
    }
}
