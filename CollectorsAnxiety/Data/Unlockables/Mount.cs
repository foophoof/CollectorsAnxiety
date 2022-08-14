using System.Collections.Generic;
using System.Linq;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class MountEntry : DataEntry<Mount> {
    private static HashSet<Mount> _uniqueMusicMounts = Injections.DataManager.Excel.GetSheet<Mount>()!
        .GroupBy(n => n.RideBGM.Row)
        .Where(g => g.Count() == 1)
        .Select(g => g.First())
        .ToHashSet();

    public MountEntry(Mount excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(this.LuminaEntry);

        this.NumberSeats = this.LuminaEntry.ExtraSeats + 1;
        this.HasActions = this.LuminaEntry.MountAction.Row != 0;
    }

    public override Item? UnlockItem { get; }
    
    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override TextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.Icon);

    public int NumberSeats { get; }
    public bool HasActions { get; }

    public bool HasUniqueMusic => _uniqueMusicMounts.Contains(this.LuminaEntry);

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsMountUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.UIPriority != 0;
    }
}