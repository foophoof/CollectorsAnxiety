﻿using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class ArmoireEntry : Unlockable<Cabinet> {
    public ArmoireEntry(Cabinet excelRow) : base(excelRow) {
        this.UnlockItem = this.LuminaEntry.Item.Value; // rly. 
    }

    public override string Name => this.LuminaEntry.Item.Value?.Singular.RawString.ToTitleCase() ??
                                   UIStrings.ErrorHandling_Unknown;

    public override Item? UnlockItem { get; }

    public string Category => this.LuminaEntry.Category.Value!.Category.Value!.Text.RawString;

    public override uint? IconId => this.UnlockItem?.Icon;
    
    public override uint SortKey => this.LuminaEntry.Order;

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsInArmoire(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.Order != 0 && (this.UnlockItem != null && this.UnlockItem.ItemUICategory.Row != 0);
    }
}