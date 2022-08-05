using CollectorsAnxiety.Util;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class TomeEntry : DataEntry<Item> {
    private readonly uint _bookId;

    public TomeEntry(Item excelRow) : base(excelRow) {
        this._bookId = excelRow.ItemAction.Value!.Data[0];
    }

    public override uint Id => this._bookId;
    public override string Name => this.LuminaEntry.Name.RawString.ToTitleCase();
    
    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);

    public override bool IsUnlocked() {
        return this.LuminaEntry.FilterGroup switch {
            23 => CollectorsAnxietyPlugin.Instance.GameState.IsMasterTomeUnlocked(this._bookId),
            30 => CollectorsAnxietyPlugin.Instance.GameState.IsFolkloreTomeUnlocked(this._bookId),
            _ => false
        };
    }

    public override bool IsValid() {
        return this.LuminaEntry.ItemAction.Value is {Type: 0x100B or 0x858};
    }
}