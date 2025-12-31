using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class TomeEntry : Unlockable<Item>
{
    public TomeEntry(Item excelRow) : base(excelRow)
    {
        Id = excelRow.ItemAction.Value!.Data[0];
        UnlockItem = excelRow;
    }

    public override uint Id { get; }
    public override string Name => LuminaEntry.Name.ExtractText().ToTitleCase();
    public override Item? UnlockItem { get; }

    public override uint? IconId => LuminaEntry.Icon;

    public override bool IsUnlocked()
    {
        return LuminaEntry.FilterGroup switch
        {
            23 => GameState.IsMasterTomeUnlocked(Id),
            30 => GameState.IsFolkloreTomeUnlocked(Id),
            _ => false,
        };
    }

    public override bool IsValid()
    {
        return LuminaEntry.ItemAction.Value is {Action.RowId: 0x100B or 0x858};
    }
}
