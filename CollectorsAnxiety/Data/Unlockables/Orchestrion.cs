using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class OrchestrionEntry : Unlockable<Orchestrion>
{
    public OrchestrionEntry(Orchestrion excelRow, UnlockItemCache unlockItemCache, ExcelSheet<OrchestrionUiparam> orchestrionUiparamsSheet) :
        base(excelRow)
    {
        UnlockItem = unlockItemCache.GetItemForObject(excelRow);
        Category = orchestrionUiparamsSheet.GetRow(excelRow.RowId).OrchestrionCategory.Value.Name.ToString();
    }

    public override string Name => LuminaEntry.Name.ExtractText().ToTitleCase();

    public override uint? IconId => 25945;

    public override Item? UnlockItem { get; }

    public string? Category { get; }

    public override bool IsUnlocked()
    {
        return GameState.IsOrchestrionUnlocked(Id);
    }

    public override bool IsValid()
    {
        return Id > 0 && Name != "";
    }
}
