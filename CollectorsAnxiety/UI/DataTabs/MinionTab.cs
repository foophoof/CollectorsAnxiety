using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class MinionTab : DataTab<MinionEntry, Companion>
{
    public override string Name => "Minions";
}
