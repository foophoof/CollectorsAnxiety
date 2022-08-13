using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class EmoteTab : BaseTab<EmoteEntry, Emote> {
    public override string Name => UIStrings.EmoteTab_Name;
}