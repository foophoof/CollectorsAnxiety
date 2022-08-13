using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class OrchestrionTab : BaseTab<OrchestrionEntry, Orchestrion> {
    public override string Name => UIStrings.OrchestrionTab_Name;
}