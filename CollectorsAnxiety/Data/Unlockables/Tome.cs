using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class TomeEntry : Unlockable<Item>
{
    public TomeEntry(Item excelRow) : base(excelRow)
    {
        this.Id = excelRow.ItemAction.Value!.Data[0];
        this.UnlockItem = excelRow;
    }

    public override uint Id { get; }
    public override string Name => this.LuminaEntry.Name.ExtractText().ToTitleCase();
    public override Item? UnlockItem { get; }

    public override uint? IconId => this.LuminaEntry.Icon;

    public override bool IsUnlocked()
    {
        return this.LuminaEntry.FilterGroup switch
        {
            23 => GameState.IsMasterTomeUnlocked(this.Id),
            30 => GameState.IsFolkloreTomeUnlocked(this.Id),
            _ => false
        };
    }

    public override bool IsValid()
    {
        return this.LuminaEntry.ItemAction.Value is {Type: 0x100B or 0x858};
    }
}
