using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class MountEntry : Unlockable<Mount>
{
    public required UniqueMusicMounts UniqueMusicMounts { protected get; init; }

    public MountEntry(Mount excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        UnlockItem = unlockItemCache.GetItemForObject(LuminaEntry);

        NumberSeats = LuminaEntry.ExtraSeats + 1;
        HasActions = LuminaEntry.MountAction.RowId != 0;
    }

    public override Item? UnlockItem { get; }

    public override string Name => LuminaEntry.Singular.ToDalamudString().ToTitleCase();

    public override uint? IconId => LuminaEntry.Icon;

    public override uint SortKey => (uint)((LuminaEntry.UIPriority << 8) + LuminaEntry.Order);

    public int NumberSeats { get; }
    public bool HasActions { get; }

    public bool HasUniqueMusic => UniqueMusicMounts.HasUniqueMusic(LuminaEntry);

    public override bool IsUnlocked()
    {
        return GameState.IsMountUnlocked(Id);
    }

    public override bool IsValid()
    {
        return LuminaEntry.UIPriority != 0;
    }
}
