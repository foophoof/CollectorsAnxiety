using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.UI.DataTabs;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Windows;

public class CollectorWindow : Window {
    public static string WindowKey => "Collector's Anxiety###mainWindow";

    public CollectorWindow() : base(WindowKey) {
        this.SizeCondition = ImGuiCond.FirstUseEver;
        this.SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(640, 480),
            MaximumSize = new Vector2(1024, 768)
        };
    }

    public readonly List<IDataTab> DataTabs = new() {
        new EmoteTab(),
        new MountTab(),
        new MinionTab(),
        new BuddyEquipTab(),
        new HairstyleTab(),
        new TomesTab(),
        new ArmoireTab(),
        new DutyTab(),
        new OrchestrionTab(),
        new OrnamentTab(),
        new FramersKitTab(),
        new GlassesTab()
    };

    private readonly List<ITab> _tabs = new();
    private readonly Stopwatch _stopwatch = new();
    private readonly string _versionString = VersionUtil.GetCurrentMajMinBuild();

    private readonly Dictionary<ITab, CrashTab> _crashTabs = new();

    public override void OnOpen() {
        base.OnOpen();

        // Load in tabs if none are found, somehow.
        if (this._tabs.Count == 0) {
            this._tabs.Add(new OverviewTab(this));
            this._tabs.AddRange(this.DataTabs);
            this._tabs.Add(new SettingsTab());
#if DEBUG
            this._tabs.Add(new DevTab());
#endif
        }
    }

    public override void Draw() {
        this.WindowName = WindowKey;
        var pbs = ImGuiHelpers.GetButtonSize(".");

        if (!Injections.ClientState.IsLoggedIn || Injections.ClientState.LocalPlayer == null) {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudOrange)) {
                ImGuiUtil.CenteredWrappedText("WARNING: A player is not logged in. Data may be invalid or incomplete.");
            }
        }

        using (ImRaii.TabBar("mainBar", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton)) {
            foreach (var tab in this._tabs) {
                using var tabItem = ImRaii.TabItem($"{tab.Name}###{tab.GetType().Name}");
                if (!tabItem)
                    continue;

                var childSize = ImGui.GetContentRegionAvail();
                var style = ImGui.GetStyle();
                var paddedY = childSize.Y - pbs.Y - 3 * style.ItemSpacing.Y + 2 * style.FramePadding.Y;

                using var child = ImRaii.Child($"##tabChild-{tab.GetType().Name}", childSize with {Y = paddedY});

                var tabToDraw = tab;

                this._stopwatch.Start();
                if (this._crashTabs.TryGetValue(tab, out var crashTab)) {
                    tabToDraw = crashTab;
                }

                try {
                    tabToDraw.Draw();
                } catch (Exception ex) {
                    // check if things are ***really*** broken
                    if (tabToDraw is CrashTab) throw;

                    Injections.PluginLog.Error(ex, $"Error drawing tab {tabToDraw.Name}!");
                    this._crashTabs[tab] = new CrashTab(tab, ex);
                }

                this._stopwatch.Stop();
            }
        }

        ImGui.Separator();

        ImGui.TextColored(ImGuiColors.DalamudGrey2, $"v{this._versionString}");

        if (Injections.PluginInterface.IsDevMenuOpen || Injections.PluginInterface.IsDev) {
            ImGui.SameLine();
            var stopwatchTime = this._stopwatch.ElapsedTicks / (double) 10000;
            var framerate = ImGui.GetIO().Framerate;
            var framesWasted = stopwatchTime / (1000 / framerate);
            ImGui.Text($"TAB RENDER: {stopwatchTime:F4}ms ({framesWasted:F2} frames @ {framerate:F0}fps)");
        }

        this._stopwatch.Reset();
    }
}
