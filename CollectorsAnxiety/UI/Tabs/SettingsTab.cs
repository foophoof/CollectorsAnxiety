using System.Globalization;
using System.Numerics;
using CollectorsAnxiety.Base;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Tabs;

public class SettingsTab : ITab {
    public string Name => $"Settings###{nameof(SettingsTab)}";

    private readonly PluginConfig _config;

    // settings cache
    private bool _hideSpoilers;
    private bool _hiddenItemsInOverview;

    public SettingsTab() {
        this._config = CollectorsAnxietyPlugin.Instance.Configuration;

        this._hideSpoilers = this._config.HideSpoilers;
        this._hiddenItemsInOverview = this._config.CountHiddenItemsInOverview;
    }

    public void Draw() {
        var childSize = ImGui.GetContentRegionAvail();
        var pbs = ImGuiHelpers.GetButtonSize("placeholder");
        var style = ImGui.GetStyle();

        var paddedY = childSize.Y - pbs.Y - 3 * style.ItemSpacing.Y + 2 * style.FramePadding.Y;
        ImGui.BeginChild("SettingsPane", childSize with {Y = paddedY});

        ImGui.Text("System Options");
        ImGui.Indent();

        if (ImGui.Checkbox("Spoiler-Free Mode", ref this._hideSpoilers)) {
            this._config.HideSpoilers = this._hideSpoilers;
            this._config.Save();
        }

        ImGuiComponents.HelpMarker("When enabled, this feature will mask all non-collected items with a special icon and spoiler text. Uncollected items will still show in counts. Turning this off may reveal items that are not yet available or otherwise spoil certain experiences.");

        if (ImGui.Checkbox("Count Hidden Items in Overview", ref this._hiddenItemsInOverview)) {
            this._config.CountHiddenItemsInOverview = this._hiddenItemsInOverview;
            this._config.Save();
        }

        ImGuiComponents.HelpMarker("When enabled, the Overview tab will include hidden items in the totals displayed and won\'t invalidate your parses. This option does not affect the progress bars on each individual tab.");

        ImGui.Unindent();
        ImGui.Spacing();

        ImGui.Text("Maintenance");
        ImGui.Indent();

        if (ImGui.Button("Unhide All Items")) {
            this._config.HiddenItems.Clear();
            this._config.Save();
        }

        ImGui.Unindent();
        ImGui.EndChild();

        // Footer buttons
        if (ImGui.Button("Plugin GitHub")) Dalamud.Utility.Util.OpenLink(Constants.GITHUB_URL);
    }
}
