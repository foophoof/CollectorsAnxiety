using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class MountEntry : Unlockable<Mount> {
    public required UniqueMusicMounts UniqueMusicMounts { protected get; init; }
    
    public MountEntry(Mount excelRow, UnlockItemCache unlockItemCache) : base(excelRow) {
        this.UnlockItem = unlockItemCache.GetItemForObject(this.LuminaEntry);

        this.NumberSeats = this.LuminaEntry.ExtraSeats + 1;
        this.HasActions = this.LuminaEntry.MountAction.RowId != 0;
    }

    public override Item? UnlockItem { get; }

    public override string Name => this.LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override uint? IconId => this.LuminaEntry.Icon;

    public override uint SortKey => (uint) ((this.LuminaEntry.UIPriority << 8) + this.LuminaEntry.Order);

    public int NumberSeats { get; }
    public bool HasActions { get; }

    public bool HasUniqueMusic => this.UniqueMusicMounts.HasUniqueMusic(this.LuminaEntry);

    public override bool IsUnlocked() {
        return GameState.IsMountUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.UIPriority != 0;
    }
}
