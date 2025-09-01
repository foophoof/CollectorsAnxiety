using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Autofac.Features.Indexed;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Services;
using CollectorsAnxiety.UI.DataTabs;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Bindings.ImGui;

namespace CollectorsAnxiety.UI.Windows;

public class CollectorWindow : Window
{
    public static string WindowKey => "Collector's Anxiety###mainWindow";

    public required IClientState ClientState { protected get; init; }
    public required IPluginLog PluginLog { protected get; init; }
    public required IDalamudPluginInterface PluginInterface { protected get; init; }

    public CollectorWindow(
        IIndex<string, IDataTab> dataTabs,
        OverviewTab overviewTab,
        SettingsTab settingsTab,
        DevTab devTab
    ) : base(WindowKey)
    {
        this.SizeCondition = ImGuiCond.FirstUseEver;
        this.SizeConstraints = new WindowSizeConstraints {MinimumSize = new Vector2(640, 480), MaximumSize = new Vector2(1024, 768)};

        this._tabs =
        [
            overviewTab,
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
            settingsTab,
        ];
#if DEBUG
        this._tabs.Add(devTab);
#endif
    }

    private readonly List<ITab> _tabs = new();
    private readonly Stopwatch _stopwatch = new();
    private readonly string _versionString = VersionUtil.GetCurrentMajMinBuild();

    private readonly Dictionary<ITab, CrashTab> _crashTabs = new();

    public override void Draw()
    {
        this.WindowName = WindowKey;
        var pbs = ImGuiHelpers.GetButtonSize(".");

        if (!this.ClientState.IsLoggedIn || this.ClientState.LocalPlayer == null)
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudOrange))
            {
                ImGuiUtil.CenteredWrappedText("WARNING: A player is not logged in. Data may be invalid or incomplete.");
            }
        }

        using (ImRaii.TabBar("mainBar", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.ListPopupButton))
        {
            foreach (var tab in this._tabs)
            {
                using var tabItem = ImRaii.TabItem($"{tab.Name}###{tab.GetType().Name}");
                if (!tabItem)
                    continue;

                var childSize = ImGui.GetContentRegionAvail();
                var style = ImGui.GetStyle();
                var paddedY = childSize.Y - pbs.Y - 3 * style.ItemSpacing.Y + 2 * style.FramePadding.Y;

                using var child = ImRaii.Child($"##tabChild-{tab.GetType().Name}", childSize with {Y = paddedY});

                var tabToDraw = tab;

                this._stopwatch.Start();
                if (this._crashTabs.TryGetValue(tab, out var crashTab))
                {
                    tabToDraw = crashTab;
                }

                try
                {
                    tabToDraw.Draw();
                }
                catch (Exception ex)
                {
                    // check if things are ***really*** broken
                    if (tabToDraw is CrashTab) throw;

                    this.PluginLog.Error(ex, $"Error drawing tab {tabToDraw.Name}!");
                    this._crashTabs[tab] = new CrashTab(tab, ex);
                }

                this._stopwatch.Stop();
            }
        }

        ImGui.Separator();

        ImGui.TextColored(ImGuiColors.DalamudGrey2, $"v{this._versionString}");

        if (this.PluginInterface.IsDevMenuOpen || this.PluginInterface.IsDev)
        {
            ImGui.SameLine();
            var stopwatchTime = this._stopwatch.ElapsedTicks / (double)10000;
            var framerate = ImGui.GetIO().Framerate;
            var framesWasted = stopwatchTime / (1000 / framerate);
            ImGui.Text($"TAB RENDER: {stopwatchTime:F4}ms ({framesWasted:F2} frames @ {framerate:F0}fps)");
        }

        this._stopwatch.Reset();
    }
}
