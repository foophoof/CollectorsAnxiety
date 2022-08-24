using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class EmoteTab : DataTab<EmoteEntry, Emote> {
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