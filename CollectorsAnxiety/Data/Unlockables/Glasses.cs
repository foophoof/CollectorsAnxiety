using System.Linq;
using CollectorsAnxiety.Game;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class GlassesEntry : Unlockable<GlassesStyle>
{
    public GlassesEntry(GlassesStyle excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        glasses = LuminaEntry.Glasses.First().Value;
        if (glasses != null)
        {
            UnlockItem = unlockItemCache.GetItemForObject(glasses.Value);
        }
    }

    public override string Name => LuminaEntry.Name.ExtractText();

    public override uint? IconId => (uint)LuminaEntry.Icon;

    public override Item? UnlockItem { get; }

    public override uint SortKey => LuminaEntry.Order;

    private Glasses? glasses { get; }

    public override bool IsUnlocked()
    {
        return glasses != null && GameState.IsGlassesUnlocked((ushort)glasses.Value.RowId);
    }

    public override bool IsValid()
    {
        return Id > 0 && Name != "";
    }
}
