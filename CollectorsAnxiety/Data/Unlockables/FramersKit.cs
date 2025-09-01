using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class FramersKitEntry : Unlockable<Item>
{
    public FramersKitEntry(Item excelRow) : base(excelRow)
    {
        Id = excelRow.AdditionalData.RowId;
        UnlockItem = excelRow;
    }

    public override uint Id { get; }
    public override string Name => LuminaEntry.Name.ExtractText().ToTitleCase();
    public override Item? UnlockItem { get; }

    public override uint? IconId => LuminaEntry.Icon;

    public override bool IsUnlocked()
    {
        return GameState.IsFramersKitUnlocked(LuminaEntry.AdditionalData.RowId);
    }

    public override bool IsValid()
    {
        return LuminaEntry.ItemAction.RowId == 2234;
    }
}
