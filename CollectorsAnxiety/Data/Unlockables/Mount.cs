using CollectorsAnxiety.Util;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class MountEntry : DataEntry<Mount> {
    public MountEntry(Mount excelRow) : base(excelRow) { }
    
    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);
    
    public int NumberSeats => 1 + this.LuminaEntry.ExtraSeats;
    
    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsMountUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.UIPriority != 0;
    }
}