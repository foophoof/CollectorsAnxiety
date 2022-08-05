using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class TomesTab : BaseTab<TomeEntry, Item> {
    public override string Name => PluginStrings.TomesTab_Name;
}