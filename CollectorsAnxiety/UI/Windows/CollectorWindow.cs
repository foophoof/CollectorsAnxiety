using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.UI.DataTabs;
using CollectorsAnxiety.UI.Tabs;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;

namespace CollectorsAnxiety.UI.Windows; 

public class CollectorWindow : Window {
    public static string WindowKey => $"{UIStrings.CollectorsAnxiety_Title}###mainWindow";

    public CollectorWindow() : base(WindowKey, ImGuiWindowFlags.None, true) {
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
        new FramersKitTab()
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
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
            ImGuiUtil.CenteredWrappedText(UIStrings.CollectorWindow_NoUserLoggedInWarning);
            ImGui.PopStyleColor();
        }

        ImGui.BeginTabBar("mainBar", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton);

        foreach (var tab in this._tabs) {
            if (ImGui.BeginTabItem($"{tab.Name}###{tab.GetType().Name}")) {
                var childSize = ImGui.GetContentRegionAvail();
                var style = ImGui.GetStyle();
                var paddedY = childSize.Y - pbs.Y - 3 * style.ItemSpacing.Y + 2 * style.FramePadding.Y;
                ImGui.BeginChild($"##tabChild-{tab.GetType().Name}", childSize with {Y = paddedY});

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
                    
                    PluginLog.Error(ex, $"Error drawing tab {tabToDraw.Name}!");
                    this._crashTabs[tab] = new CrashTab(tab, ex);
                }

                this._stopwatch.Stop();
                
                ImGui.EndChild();

                ImGui.EndTabItem();
            }
        }

        ImGui.EndTabBar();
        
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