using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class MinionTab : DataTab<MinionEntry, Companion> {
    public override string Name => UIStrings.MinionTab_Name;
}
