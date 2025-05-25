using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;

namespace CollectorsAnxiety.UI.Tabs;

public class OverviewTab(IIndex<string, IDataTab> dataTabs) : ITab {

    public string Name => "Overview";
    
    public required PluginConfig PluginConfig { protected get; init; }

    private readonly List<IDataTab> DataTabs = [
        dataTabs["Emote"],
        dataTabs["Mount"],
        dataTabs["Minion"],
        dataTabs["BuddyEquip"],
        dataTabs["Hairstyle"],
        dataTabs["Tomes"],
        dataTabs["Armoire"],
        dataTabs["Duty"],
        dataTabs["Orchestrion"],
        dataTabs["Ornament"],
        dataTabs["FramersKit"],
        dataTabs["Glasses"],
    ];

    public void Draw() {
        var grandTotalUnlocked = 0;
        var grandTotalItems = 0;
        var tainted = false;

        var labelWidth = this.DataTabs.Max(t => ImGui.CalcTextSize(t.Name).X) + 10;
        var forceShowHidden = this.PluginConfig.CountHiddenItemsInOverview;

        using (var table = ImRaii.Table("##overview", 2)) {
            if (table) {
                ImGui.TableSetupColumn("Label", ImGuiTableColumnFlags.WidthFixed, labelWidth);
                ImGui.TableSetupColumn("ProgressBar");
                // ImGui.TableHeadersRow();
                ImGui.TableNextRow();

                foreach (var tab in this.DataTabs) {
                    if (!tab.ShowInOverview) continue;

                    var controller = tab.GetController();
                    var counts = controller.GetCounts(!forceShowHidden);

                    var percentage = counts.UnlockedCount / (double) counts.TotalCount;

                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextColored(ImGuiUtil.GetBarseColor(percentage), tab.Name);
                    if (!forceShowHidden && controller.ParseTainted) {
                        ImGui.SameLine();
                        ImGui.TextColored(ImGuiColors.DalamudRed, "*");
                        tainted = true;
                    }

                    ImGui.TableSetColumnIndex(1);
                    ImGuiUtil.CompletionProgressBar(counts.UnlockedCount, counts.TotalCount);

                    grandTotalUnlocked += counts.UnlockedCount;
                    grandTotalItems += counts.TotalCount;
                    ImGui.TableNextRow();
                }

                ImGui.TableNextRow();

                ImGui.TableSetColumnIndex(0);
                ImGui.TextColored(ImGuiUtil.GetBarseColor(grandTotalUnlocked / (double) grandTotalItems), "TOTAL");
                if (tainted) {
                    ImGui.SameLine();
                    ImGui.TextColored(ImGuiColors.DalamudRed, "*");
                    tainted = true;
                }

                ImGui.TableSetColumnIndex(1);
                ImGuiUtil.CompletionProgressBar(grandTotalUnlocked, grandTotalItems);
            }
        }

        if (tainted) {
            using var indent = ImRaii.PushIndent();
            ImGui.TextColored(ImGuiColors.DalamudRed, "*");
            ImGui.SameLine();
            ImGui.TextWrapped("A red asterisk by a metric means that the parse is \"tainted.\" Generally, this " +
                              "means that the player has chosen to hide a specific item because they felt it was " +
                              "\"too hard\" or maybe even \"impossible.\" Psh, excuses. Unfortunately for them, " +
                              "parses are not merciful. They shall wear the badge of shame forever. Or, at least, " +
                              "until they un-hide the item.");
        }
    }
}
