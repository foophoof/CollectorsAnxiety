using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class GlassesTab : DataTab<GlassesEntry, GlassesStyle> {
    public override string Name => UIStrings.GlassesTab_Name;
}
