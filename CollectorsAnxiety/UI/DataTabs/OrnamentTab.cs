using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class OrnamentTab : DataTab<OrnamentEntry, Ornament> {
    public override string Name => UIStrings.OrnamentTab_Name;
}
