using CollectorsAnxiety.Game;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class BardingEntry : Unlockable<BuddyEquip>
{
    public BardingEntry(BuddyEquip excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        UnlockItem = unlockItemCache.GetItemForObject(excelRow);
    }

    public override string Name => LuminaEntry.Name.ExtractText();

    public override uint? IconId => LuminaEntry.IconHead;

    public override Item? UnlockItem { get; }

    public override uint SortKey => LuminaEntry.Order;

    public override bool IsUnlocked()
    {
        return GameState.IsBuddyEquipUnlocked(Id);
    }

    public override bool IsValid()
    {
        return LuminaEntry.Order != 0;
    }
}
