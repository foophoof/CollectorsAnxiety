using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class OrchestrionEntry : Unlockable<Orchestrion> {
    public OrchestrionEntry(Orchestrion excelRow, UnlockItemCache unlockItemCache, ExcelSheet<OrchestrionUiparam> orchestrionUiparamsSheet) :
        base(excelRow) {
        this.UnlockItem = unlockItemCache.GetItemForObject(excelRow);
        this.Category = orchestrionUiparamsSheet.GetRow(excelRow.RowId).OrchestrionCategory.Value.Name.ToString();
    }

    public override string Name => this.LuminaEntry.Name.ExtractText().ToTitleCase();

    public override uint? IconId => 25945;

    public override Item? UnlockItem { get; }

    public string? Category { get; }

    public override bool IsUnlocked() {
        return GameState.IsOrchestrionUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.Id > 0 && this.Name != "";
    }
}
