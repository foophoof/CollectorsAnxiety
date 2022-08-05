using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.Windows;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Tabs; 

public class OverviewTab : ITab {
    private CollectorWindow _baseWindow;

    public OverviewTab(CollectorWindow window) {
        this._baseWindow = window;
    }

    public void Dispose() {
        
    }
    
    public string Name => PluginStrings.OverviewTab_Name;
    public void Draw() {
        var grandTotalUnlocked = 0;
        var grandTotalItems = 0;
        var tainted = false;
        
        if (ImGui.BeginTable("##overview", 2)) {
            ImGui.TableSetupColumn("Label", ImGuiTableColumnFlags.WidthFixed, 128);
            ImGui.TableSetupColumn("ProgressBar");
            // ImGui.TableHeadersRow();
            ImGui.TableNextRow();

            foreach (var tab in this._baseWindow.DataTabs) {
                if (!tab.ShowInOverview) continue;

                var controller = tab.GetController();
                var counts = controller.GetCounts();
                
                var percentage = counts.UnlockedCount / (double) counts.TotalCount;

                ImGui.TableSetColumnIndex(0);
                ImGui.TextColored(ImGuiUtil.GetBarseColor(percentage), tab.Name);
                if (controller.ParseTainted) {
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
            // ImGui.PushStyleColor(ImGuiCol.PlotHistogram, ImGuiColors.HealerGreen);
            ImGuiUtil.CompletionProgressBar(grandTotalUnlocked, grandTotalItems);
            // ImGui.PopStyleColor();
            
            ImGui.EndTable();
        }

        if (tainted) {
            ImGui.Indent();
            ImGui.TextColored(ImGuiColors.DalamudRed, "*");
            ImGui.SameLine();
            ImGui.TextWrapped("A red asterisk by a metric means that the parse is \"tainted.\" Generally, this " +
                              "means that the player has chosen to hide a specific item because they felt it was " +
                              "\"too hard\" or maybe even \"impossible.\" Psh, excuses. Unfortunately for them, " +
                              "parses are not merciful. They shall wear the badge of shame forever. Or, at least, " +
                              "until they un-hide the item.");
            ImGui.Unindent();
        }
    }
}