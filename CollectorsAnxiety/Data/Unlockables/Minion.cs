using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class MinionEntry : Unlockable<Companion> {
    public MinionEntry(Companion excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(excelRow);
    }

    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override Item? UnlockItem { get; }

    public override uint? IconId => this.LuminaEntry.Icon;

    public override uint SortKey => this.LuminaEntry.Order;

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsMinionUnlocked(this.Id);
    }

    public override bool IsValid() {
        if (this.LuminaEntry.Order == 0)
            return false;

        // Exclude GC and WOL trio, as they aren't technically usable.
        return this.Id is (< 68 or > 70) and (< 72 or > 74);
    }
}
