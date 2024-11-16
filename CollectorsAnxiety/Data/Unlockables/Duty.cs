using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class DutyEntry : Unlockable<ContentFinderCondition> {
    public DutyEntry(ContentFinderCondition excelRow) : base(excelRow) { }

    public override string Name => this.LuminaEntry.Name.ToString().ToTitleCase();

    public override uint SortKey => (this.LuminaEntry.ContentType.RowId << 16) + this.LuminaEntry.SortKey;

    public override uint? IconId => this.GetIconId();

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsDutyUnlocked(this.LuminaEntry.Content.RowId);
    }

    public bool IsCompleted() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsDutyCompleted(this.LuminaEntry.Content.RowId);
    }

    public override bool IsValid() {
        return this.LuminaEntry.SortKey != 0 &&
               this.LuminaEntry.SortKey < 9001 &&
               this.LuminaEntry.ContentType.RowId is (> 0 and <= 5) or 28 &&
               !this.Name.IsNullOrEmpty();
    }

    public string Category => this.LuminaEntry.ContentType.ValueNullable?.Name.ToString() ?? "Unknown";

    public int MinLevel => this.LuminaEntry.ClassJobLevelRequired;

    public string? UnlockQuestName => this.LuminaEntry.UnlockQuest.Value.Name.ToString();

    private uint GetIconId() {
        if (this.LuminaEntry.Icon != 0)
            return this.LuminaEntry.Icon;

        return this.LuminaEntry.ContentType.RowId switch {
            2 => 60831,
            3 => 60833,
            4 => 60834,
            5 => 60832,
            28 => 60855,
            _ => 0
        };
    }
}
