using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.UI.DataTabs; 

public class HairstyleTab : BaseTab<HairstyleEntry, CharaMakeCustomize> {
    public HairstyleTab() {
        this.Controller = new HairstyleController();
    }
    
    public override string Name => UIStrings.HairstyleTab_Name;
}