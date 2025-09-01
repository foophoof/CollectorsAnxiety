using CollectorsAnxiety.Game;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class EmoteEntry : Unlockable<Emote>
{
    public EmoteEntry(Emote excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        this.UnlockItem = unlockItemCache.GetItemForUnlockLink(this.LuminaEntry.UnlockLink);
    }

    public override Item? UnlockItem { get; }

    public override string Name => this.LuminaEntry.Name.ToDalamudString().ToTitleCase();

    public override uint? IconId => this.LuminaEntry.Icon;

    public override uint SortKey => this.LuminaEntry.Order;

    public string Category => this.LuminaEntry.EmoteCategory.ValueNullable?.Name.ToDalamudString().ToTitleCase() ?? "Unknown";


    public override bool IsUnlocked()
    {
        return GameState.IsEmoteUnlocked(this.LuminaEntry);
    }

    public override bool IsValid()
    {
        return this.LuminaEntry.UnlockLink != 0 && this.LuminaEntry.Order != 0;
    }
}
