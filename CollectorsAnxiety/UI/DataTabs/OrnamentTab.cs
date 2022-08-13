using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class OrnamentTab : BaseTab<OrnamentEntry, Ornament> {
    public override string Name => UIStrings.OrnamentTab_Name;
}