using System.Linq;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class GlassesEntry : Unlockable<GlassesStyle> {
    public GlassesEntry(GlassesStyle excelRow) : base(excelRow) {
        this.glasses = this.LuminaEntry.Glasses.First().Value;
        if (this.glasses != null)
            this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(this.glasses.Value);
    }

    public override string Name => this.LuminaEntry.Name.ExtractText();

    public override uint? IconId => (uint) this.LuminaEntry.Icon;

    public override Item? UnlockItem { get; }

    public override uint SortKey => this.LuminaEntry.Order;

    private Glasses? glasses { get; }

    public override bool IsUnlocked() {
        return this.glasses != null && CollectorsAnxietyPlugin.Instance.GameState.IsGlassesUnlocked((ushort) this.glasses.Value.RowId);
    }

    public override bool IsValid() {
        return this.Id > 0 && this.Name != "";
    }
}
