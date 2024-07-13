using System.Linq;
using Dalamud.Utility;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class GlassesEntry : Unlockable<GlassesStyle> {
    public GlassesEntry(GlassesStyle excelRow) : base(excelRow) {
        this.glasses = this.LuminaEntry.Glasses.First().Value;
        if (this.glasses != null)
            this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(this.glasses);
    }

    public override string Name => this.LuminaEntry.Name;

    public override uint? IconId => (uint)this.LuminaEntry.Icon;
    
    public override Item? UnlockItem { get; }

    public override uint SortKey => this.LuminaEntry.Order;
    
    private Glasses? glasses { get; }

    public override bool IsUnlocked() {
        return this.glasses != null && CollectorsAnxietyPlugin.Instance.GameState.IsGlassesUnlocked((ushort)this.glasses.RowId);
    }

    public override bool IsValid() {
        return this.Id > 0 && this.Name != "";
    }
}