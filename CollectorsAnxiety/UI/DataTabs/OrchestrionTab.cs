using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class OrchestrionTab : DataTab<OrchestrionEntry, Orchestrion> {
    public override string Name => "Orchestrion Rolls";

    protected override string? GetTagline(OrchestrionEntry entry) {
        return entry.Category;
    }
}
