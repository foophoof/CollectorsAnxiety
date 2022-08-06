using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class MountTab : BaseTab<MountEntry, Mount> {
    public override string Name => PluginStrings.MountTab_Name;
}