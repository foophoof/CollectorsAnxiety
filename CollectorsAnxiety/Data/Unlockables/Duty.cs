using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class DutyEntry : Unlockable<ContentFinderCondition>
{
    public DutyEntry(ContentFinderCondition excelRow) : base(excelRow) { }

    public override string Name => LuminaEntry.Name.ToString().ToTitleCase();

    public override uint SortKey => (LuminaEntry.ContentType.RowId << 16) + LuminaEntry.SortKey;

    public override uint? IconId => GetIconId();

    public override bool IsUnlocked()
    {
        return GameState.IsDutyUnlocked(LuminaEntry.Content.RowId);
    }

    public bool IsCompleted()
    {
        return GameState.IsDutyCompleted(LuminaEntry.Content.RowId);
    }

    public override bool IsValid()
    {
        return LuminaEntry.SortKey != 0 &&
               LuminaEntry.SortKey < 9001 &&
               LuminaEntry.ContentType.RowId is > 0 and <= 5 or 28 &&
               !Name.IsNullOrEmpty();
    }

    public string Category => LuminaEntry.ContentType.ValueNullable?.Name.ToString() ?? "Unknown";

    public int MinLevel => LuminaEntry.ClassJobLevelRequired;

    public string? UnlockQuestName => LuminaEntry.UnlockQuest.Value.Name.ToString();

    private uint GetIconId()
    {
        if (LuminaEntry.Icon != 0)
        {
            return LuminaEntry.Icon;
        }

        return LuminaEntry.ContentType.RowId switch
        {
            2 => 60831,
            3 => 60833,
            4 => 60834,
            5 => 60832,
            28 => 60855,
            _ => 0,
        };
    }
}
