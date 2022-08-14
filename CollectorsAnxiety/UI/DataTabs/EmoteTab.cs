using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class EmoteTab : BaseTab<EmoteEntry, Emote> {
    public override string Name => UIStrings.EmoteTab_Name;

    protected override string GetTagline(EmoteEntry entry) {
        var tagline = entry.Category;
        
        var unlockItem = entry.UnlockItem;
        if (unlockItem != null) {
            tagline += $" | {unlockItem.Name.RawString.ToTitleCase()}";
        }

        return tagline;
    }
}