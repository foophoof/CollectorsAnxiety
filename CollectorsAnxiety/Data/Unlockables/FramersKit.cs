using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class FramersKitEntry : Unlockable<Item> {
    public FramersKitEntry(Item excelRow) : base(excelRow) {
        this.Id = excelRow.AdditionalData.RowId;
        this.UnlockItem = excelRow;
    }

    public override uint Id { get; }
    public override string Name => this.LuminaEntry.Name.ExtractText().ToTitleCase();
    public override Item? UnlockItem { get; }

    public override uint? IconId => this.LuminaEntry.Icon;

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsFramersKitUnlocked(this.LuminaEntry.AdditionalData.RowId);
    }

    public override bool IsValid() {
        return this.LuminaEntry.ItemAction.RowId == 2234;
    }
}
