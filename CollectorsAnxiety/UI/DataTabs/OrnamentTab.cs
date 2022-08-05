using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class OrnamentTab : BaseTab<OrnamentEntry, Ornament> {
    public override string Name => PluginStrings.OrnamentTab_Name;
}