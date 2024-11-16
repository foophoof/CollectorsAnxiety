using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class TomesTab : DataTab<TomeEntry, Item> {
    public override string Name => "Tomes";
}
