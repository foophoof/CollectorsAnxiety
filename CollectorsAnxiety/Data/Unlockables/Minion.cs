using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class MinionEntry : Unlockable<Companion>
{
    public MinionEntry(Companion excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        UnlockItem = unlockItemCache.GetItemForObject(excelRow);
    }

    public override string Name => LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override Item? UnlockItem { get; }

    public override uint? IconId => LuminaEntry.Icon;

    public override uint SortKey => LuminaEntry.Order;

    public override bool IsUnlocked()
    {
        return GameState.IsMinionUnlocked(Id);
    }

    public override bool IsValid()
    {
        if (LuminaEntry.Order == 0)
        {
            return false;
        }

        // Exclude GC and WOL trio, as they aren't technically usable.
        return Id is (< 68 or > 70) and (< 72 or > 74);
    }
}
