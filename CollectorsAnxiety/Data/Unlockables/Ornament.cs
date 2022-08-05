using CollectorsAnxiety.Util;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class OrnamentEntry : DataEntry<Ornament> {
    public OrnamentEntry(Ornament excelRow) : base(excelRow) { }
    
    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsOrnamentUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.Model > 0;
    }
}