using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class BuddyEquipTab : BaseTab<BardingEntry, BuddyEquip> {
    public override string Name => PluginStrings.BuddyEquipTab_Name;
}