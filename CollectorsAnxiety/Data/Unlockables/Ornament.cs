using CollectorsAnxiety.Util;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class OrnamentEntry : Unlockable<Ornament> {
    public OrnamentEntry(Ornament excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(excelRow);
    }
    
    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);

    public override Item? UnlockItem { get; }

    public override uint SortKey => (uint) this.LuminaEntry.Order;

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsOrnamentUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.Model > 0;
    }
}