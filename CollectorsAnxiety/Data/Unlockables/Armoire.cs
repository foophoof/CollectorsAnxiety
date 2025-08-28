using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class ArmoireEntry : Unlockable<Cabinet> {
    public ArmoireEntry(Cabinet excelRow) : base(excelRow) {
        this.UnlockItem = this.LuminaEntry.Item.Value; // rly. 
    }

    public override string Name => this.LuminaEntry.Item.ValueNullable?.Singular.ExtractText().ToTitleCase() ??
                                   "Unknown";

    public override Item? UnlockItem { get; }

    public string Category => this.LuminaEntry.Category.Value.Category.Value.Text.ExtractText();

    public override uint? IconId => this.UnlockItem?.Icon;

    public override uint SortKey => this.LuminaEntry.Order;

    public override bool IsUnlocked() {
        return GameState.IsInArmoire(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.Order != 0 && this.UnlockItem != null && this.UnlockItem.Value.ItemUICategory.RowId != 0;
    }
}
