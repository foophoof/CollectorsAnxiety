using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class FramersKitTab : DataTab<FramersKitEntry, Item> {
    public override string Name => UIStrings.FramersKitTab_Name;
    
    // Excluded from the Overview because certain Framer's Kits can be missed forever (?).
    public override bool ShowInOverview => false;
}