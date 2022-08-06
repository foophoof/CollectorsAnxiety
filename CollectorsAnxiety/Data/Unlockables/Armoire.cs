using CollectorsAnxiety.Util;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class ArmoireEntry : DataEntry<Cabinet> {
    public ArmoireEntry(Cabinet excelRow) : base(excelRow) { }

    public override string Name => this.LuminaEntry.Item.Value?.Singular.RawString.ToTitleCase() ?? "Unknown";
    public string Category => this.LuminaEntry.Category.Value!.Category.Value!.Text.RawString;

    public override TextureWrap? Icon {
        get {
            var item = this.LuminaEntry.Item.Value;

            return item != null ? CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(item.Icon) : null;
        }
    }


    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsInArmoire(this.Id);
    }

    public override bool IsValid() {
        var itemValue = this.LuminaEntry.Item.Value;
        
        return this.LuminaEntry.Order != 0 && (itemValue != null && itemValue.ItemUICategory.Row != 0);
    }
}