using CollectorsAnxiety.Base;
using CollectorsAnxiety.Util;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class OrchestrionEntry : Unlockable<Orchestrion> {
    public OrchestrionEntry(Orchestrion excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(excelRow);
        this.Category = Injections.DataManager.GetExcelSheet<OrchestrionUiparam>()!
            .GetRow(excelRow.RowId)?.OrchestrionCategory.Value?.Name.ToString();
    }
    
    public override string Name => this.LuminaEntry.Name.RawString.ToTitleCase();
    
    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(25945);

    public override Item? UnlockItem { get; }

    public string? Category { get; }

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsOrchestrionUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.Id > 0 && this.Name != "";
    }
}