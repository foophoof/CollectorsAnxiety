using CollectorsAnxiety.Util;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class EmoteEntry : DataEntry<Emote> {
    public EmoteEntry(Emote excelRow) : base(excelRow) { }
    
    public override string Name => this.LuminaEntry.Name.ToDalamudString().ToTitleCase();

    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsEmoteUnlocked(this.LuminaEntry);
    }

    public override bool IsValid() {
        return this.LuminaEntry.UnlockLink != 0 && this.LuminaEntry.Order != 0;
    }
}