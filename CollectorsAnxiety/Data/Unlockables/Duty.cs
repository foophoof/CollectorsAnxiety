using CollectorsAnxiety.Util;
using Dalamud.Interface.Internal;
using Dalamud.Utility;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class DutyEntry : Unlockable<ContentFinderCondition> {
    public DutyEntry(ContentFinderCondition excelRow) : base(excelRow) { }

    public override string Name => this.LuminaEntry.Name.ToString().ToTitleCase();
    
    public override uint SortKey => (this.LuminaEntry.ContentType.Row << 16) + this.LuminaEntry.SortKey;

    public override IDalamudTextureWrap? Icon => CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.GetIconId());

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsDutyUnlocked(this.LuminaEntry.Content);
    }

    public bool IsCompleted() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsDutyCompleted(this.LuminaEntry.Content);
    }

    public override bool IsValid() {
        return this.LuminaEntry.SortKey != 0 &&
               this.LuminaEntry.SortKey < 9001 &&
               this.LuminaEntry.ContentType.Row is (> 0 and <= 5) or 28 && 
               !this.Name.IsNullOrEmpty();
    }

    public string Category => this.LuminaEntry.ContentType.Value?.Name.ToString() ?? "Unknown";

    public int MinLevel => this.LuminaEntry.ClassJobLevelRequired;

    public string? UnlockQuestName => this.LuminaEntry.UnlockQuest.Value?.Name.ToString();

    private int GetIconId() {
        if (this.LuminaEntry.Icon != 0)
            return (int) this.LuminaEntry.Icon;

        return this.LuminaEntry.ContentType.Row switch {
            2 => 60831,
            3 => 60833,
            4 => 60834,
            5 => 60832,
            28 => 60855,
            _ => 0,
        };
    }
}