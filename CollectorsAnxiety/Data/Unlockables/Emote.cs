using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class EmoteEntry : Unlockable<Emote>
{
    public EmoteEntry(Emote excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        UnlockItem = unlockItemCache.GetItemForUnlockLink(LuminaEntry.UnlockLink);
    }

    public override Item? UnlockItem { get; }

    public override string Name => LuminaEntry.Name.ToDalamudString().ToTitleCase();

    public override uint? IconId => LuminaEntry.Icon;

    public override uint SortKey => LuminaEntry.Order;

    public string Category => LuminaEntry.EmoteCategory.ValueNullable?.Name.ToDalamudString().ToTitleCase() ?? "Unknown";


    public override bool IsUnlocked()
    {
        return GameState.IsEmoteUnlocked(LuminaEntry);
    }

    public override bool IsValid()
    {
        return LuminaEntry.UnlockLink != 0 && LuminaEntry.Order != 0;
    }
}
