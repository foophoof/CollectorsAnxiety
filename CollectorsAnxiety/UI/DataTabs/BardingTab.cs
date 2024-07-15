using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class BuddyEquipTab : DataTab<BardingEntry, BuddyEquip> {
    public override string Name => UIStrings.BuddyEquipTab_Name;
}
