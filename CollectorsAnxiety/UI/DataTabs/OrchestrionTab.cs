using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class OrchestrionTab : DataTab<OrchestrionEntry, Orchestrion> {
    public override string Name => UIStrings.OrchestrionTab_Name;

    protected override string? GetTagline(OrchestrionEntry entry) {
        return entry.Category;
    }
}
