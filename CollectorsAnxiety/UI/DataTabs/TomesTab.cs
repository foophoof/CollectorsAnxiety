using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class TomesTab : DataTab<TomeEntry, Item> {
    public override string Name => UIStrings.TomesTab_Name;
}
