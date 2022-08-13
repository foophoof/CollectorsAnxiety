using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class MinionTab : BaseTab<MinionEntry, Companion> {
    public override string Name => UIStrings.MinionTab_Name;
}