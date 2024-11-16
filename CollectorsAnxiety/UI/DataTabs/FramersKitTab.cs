using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class FramersKitTab : DataTab<FramersKitEntry, Item> {
    public override string Name => "Framer's Kits";

    // Excluded from the Overview because certain Framer's Kits can be missed forever (?).
    public override bool ShowInOverview => false;
}
