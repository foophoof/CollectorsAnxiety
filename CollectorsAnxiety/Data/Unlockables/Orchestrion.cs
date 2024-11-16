﻿using CollectorsAnxiety.Base;
using CollectorsAnxiety.Util;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class OrchestrionEntry : Unlockable<Orchestrion> {
    public OrchestrionEntry(Orchestrion excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(excelRow);
        this.Category = Injections.DataManager.GetExcelSheet<OrchestrionUiparam>()
            .GetRow(excelRow.RowId).OrchestrionCategory.Value.Name.ToString();
    }

    public override string Name => this.LuminaEntry.Name.ExtractText().ToTitleCase();

    public override uint? IconId => 25945;

    public override Item? UnlockItem { get; }

    public string? Category { get; }

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsOrchestrionUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.Id > 0 && this.Name != "";
    }
}
