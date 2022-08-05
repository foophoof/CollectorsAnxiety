﻿using CollectorsAnxiety.Util;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class MinionEntry : DataEntry<Companion> {
    public MinionEntry(Companion excelRow) : base(excelRow) { }
    
    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);
    
    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsMinionUnlocked(this.Id);
    }

    public override bool IsValid() {
        if (this.LuminaEntry.Order == 0)
            return false;

        // Exclude GC and WOL trio, as they aren't technically usable.
        return this.Id is (< 68 or > 70) and (< 72 or > 74);
    }
}