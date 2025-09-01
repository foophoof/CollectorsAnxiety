using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class OrnamentEntry : Unlockable<Ornament>
{
    public OrnamentEntry(Ornament excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        UnlockItem = unlockItemCache.GetItemForObject(excelRow);
    }

    public override string Name => LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override uint? IconId => LuminaEntry.Icon;

    public override Item? UnlockItem { get; }

    public override uint SortKey => (uint)LuminaEntry.Order;

    public override bool IsUnlocked()
    {
        return GameState.IsOrnamentUnlocked(Id);
    }

    public override bool IsValid()
    {
        return LuminaEntry.Model > 0;
    }
}
