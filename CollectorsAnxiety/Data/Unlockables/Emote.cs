using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class EmoteEntry : Unlockable<Emote> {
    public EmoteEntry(Emote excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForUnlockLink(this.LuminaEntry.UnlockLink);
    }

    public override Item? UnlockItem { get; }

    public override string Name => this.LuminaEntry.Name.ToDalamudString().ToTitleCase();

    public override uint? IconId => this.LuminaEntry.Icon;

    public override uint SortKey => this.LuminaEntry.Order;

    public string Category => this.LuminaEntry.EmoteCategory.ValueNullable?.Name.ToDalamudString().ToTitleCase() ?? "Unknown";


    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsEmoteUnlocked(this.LuminaEntry);
    }

    public override bool IsValid() {
        return this.LuminaEntry.UnlockLink != 0 && this.LuminaEntry.Order != 0;
    }
}
