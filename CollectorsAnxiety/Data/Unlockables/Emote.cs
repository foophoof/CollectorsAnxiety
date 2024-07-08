using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Utility;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class EmoteEntry : Unlockable<Emote> {
    public EmoteEntry(Emote excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForUnlockLink(this.LuminaEntry.UnlockLink);
    }
    
    public override Item? UnlockItem { get; }
    
    public override string Name => this.LuminaEntry.Name.ToDalamudString().ToTitleCase();

    public override uint? IconId => this.LuminaEntry.Icon; 

    public override uint SortKey => this.LuminaEntry.Order;

    public string Category => this.LuminaEntry.EmoteCategory.Value?.Name.ToDalamudString().ToTitleCase() ?? UIStrings.ErrorHandling_Unknown;
    

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsEmoteUnlocked(this.LuminaEntry);
    }

    public override bool IsValid() {
        return this.LuminaEntry.UnlockLink != 0 && this.LuminaEntry.Order != 0;
    }
}