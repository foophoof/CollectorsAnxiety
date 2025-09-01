using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class ArmoireEntry : Unlockable<Cabinet>
{
    public ArmoireEntry(Cabinet excelRow) : base(excelRow)
    {
        UnlockItem = LuminaEntry.Item.Value; // rly. 
    }

    public override string Name => LuminaEntry.Item.ValueNullable?.Singular.ExtractText().ToTitleCase() ??
                                   "Unknown";

    public override Item? UnlockItem { get; }

    public string Category => LuminaEntry.Category.Value.Category.Value.Text.ExtractText();

    public override uint? IconId => UnlockItem?.Icon;

    public override uint SortKey => LuminaEntry.Order;

    public override bool IsUnlocked()
    {
        return GameState.IsInArmoire(Id);
    }

    public override bool IsValid()
    {
        return LuminaEntry.Order != 0 && UnlockItem != null && UnlockItem.Value.ItemUICategory.RowId != 0;
    }
}
