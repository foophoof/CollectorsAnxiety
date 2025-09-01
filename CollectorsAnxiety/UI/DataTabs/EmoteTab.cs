using CollectorsAnxiety.Data.Unlockables;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.UI.DataTabs;

public class EmoteTab : DataTab<EmoteEntry, Emote>
{
    public override string Name => "Emotes";

    protected override string GetTagline(EmoteEntry entry)
    {
        var tagline = entry.Category;

        var unlockItem = entry.UnlockItem;
        if (unlockItem != null)
        {
            tagline += $" | {unlockItem.Value.Name.ToTitleCase()}";
        }

        return tagline;
    }

    protected override void DrawDevContextMenuItems(EmoteEntry entry)
    {
        ImGui.MenuItem($"UnlockLink: {entry.LuminaEntry.UnlockLink}", false);
    }
}
