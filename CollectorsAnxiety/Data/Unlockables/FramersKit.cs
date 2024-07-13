using CollectorsAnxiety.Util;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class FramersKitEntry : Unlockable<Item> {
    public FramersKitEntry(Item excelRow) : base(excelRow) {
        this.Id = excelRow.AdditionalData;
        this.UnlockItem = excelRow;
    }

    public override uint Id { get; }
    public override string Name => this.LuminaEntry.Name.RawString.ToTitleCase();
    public override Item? UnlockItem { get; }

    public override uint? IconId => this.LuminaEntry.Icon;

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsFramersKitUnlocked(this.LuminaEntry.AdditionalData);
    }

    public override bool IsValid() {
        return this.LuminaEntry.ItemAction.Row == 2234;
    }
}