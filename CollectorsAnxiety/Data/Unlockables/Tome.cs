using CollectorsAnxiety.Util;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class TomeEntry : DataEntry<Item> {
    public TomeEntry(Item excelRow) : base(excelRow) {
        this.Id = excelRow.ItemAction.Value!.Data[0];
    }

    public override uint Id { get; }
    public override string Name => this.LuminaEntry.Name.RawString.ToTitleCase();
    
    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);

    public override bool IsUnlocked() {
        return this.LuminaEntry.FilterGroup switch {
            23 => CollectorsAnxietyPlugin.Instance.GameState.IsMasterTomeUnlocked(this.Id),
            30 => CollectorsAnxietyPlugin.Instance.GameState.IsFolkloreTomeUnlocked(this.Id),
            _ => false
        };
    }

    public override bool IsValid() {
        return this.LuminaEntry.ItemAction.Value is {Type: 0x100B or 0x858};
    }
}