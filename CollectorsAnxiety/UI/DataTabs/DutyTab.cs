using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class DutyTab : DataTab<DutyEntry, ContentFinderCondition>
{
    public override string Name => "Duty";

    protected override TableColumn[] ExtraColumns => new[] {new TableColumn("Cleared", ImGuiTableColumnFlags.WidthFixed, 48)};

    protected override string? GetTagline(DutyEntry entry)
    {
        var tagline = entry.Category;

        if (entry.MinLevel > 0)
        {
            tagline += $" (Level {entry.MinLevel})";
        }

        return tagline;
    }

    protected override void DrawExtraColumns(DutyEntry entry)
    {
        ImGui.TableSetColumnIndex(4);
        var contentCleared = entry.IsCompleted();
        ImGui.Checkbox("", ref contentCleared);
    }

    protected override void DrawEntryIcons(DutyEntry entry)
    {
        base.DrawEntryIcons(entry);

        if (entry.LuminaEntry.AllianceRoulette)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.PeopleGroup, "Alliance Raid");

        if (entry.LuminaEntry.LevelingRoulette)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Rocket, "In Leveling Roulette");

        if (entry.LuminaEntry.MentorRoulette)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Crown, "In Mentor Roulette");

        if (entry.LuminaEntry.DutyRecorderAllowed)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.PlayCircle, "Duty Recorder Enabled");

        if (entry.LuminaEntry.AllowExplorerMode)
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Compass, "Explorer Mode Allowed");
    }
}
