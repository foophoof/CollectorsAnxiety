using System.Linq;
using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class HairstyleTab : DataTab<HairstyleEntry, CharaMakeCustomize>
{
    public override string Name => "Appearances";

    protected override void DrawEntryIcons(HairstyleEntry entry)
    {
        base.DrawEntryIcons(entry);

        if (entry.WearableByMale && !entry.WearableByFemale)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Mars, "Limited to male characters");

        if (entry.WearableByFemale && !entry.WearableByMale)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Venus, "Limited to female characters");

        var maleHrothgar = entry.WearableByMaleRaceIDs.Contains(GameCompat.PlayerRace.Hrothgar);
        var femaleHrothgar = entry.WearableByFemaleRaceIDs.Contains(GameCompat.PlayerRace.Hrothgar);
        var maleViera = entry.WearableByMaleRaceIDs.Contains(GameCompat.PlayerRace.Viera);
        var femaleViera = entry.WearableByFemaleRaceIDs.Contains(GameCompat.PlayerRace.Viera);

        if (maleHrothgar && femaleHrothgar)
        {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, "Available to Hrothgar characters");
        }
        else if (maleHrothgar && !femaleHrothgar)
        {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, "Available to male Hrothgar characters");
        }
        else if (!maleHrothgar && femaleHrothgar)
        {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Paw, "Available to female Hrothgar characters");
        }

        if (maleViera && femaleViera)
        {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, "Available to Viera characters");
        }
        else if (maleViera && !femaleViera)
        {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, "Available to male Viera characters");
        }
        else if (!maleViera && femaleViera)
        {
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Carrot, "Available to female Viera characters");
        }
    }

    protected override void DrawDevContextMenuItems(HairstyleEntry entry)
    {
        var matrList = entry.WearableByMaleRaceIDs.ToList();
        ImGui.MenuItem("Available To Races (Male):", false);
        foreach (var chunk in matrList.ChunksOf(3))
        {
            ImGui.MenuItem("   " + string.Join(", ", chunk), false);
        }

        var fatrList = entry.WearableByFemaleRaceIDs.ToList();
        ImGui.MenuItem("Available To Races (Female):", false);
        foreach (var chunk in fatrList.ChunksOf(3))
        {
            ImGui.MenuItem("   " + string.Join(", ", chunk), false);
        }
    }
}
