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
using ImGuiNET;

namespace CollectorsAnxiety.UI.Windows; 

public class CollectorWindow : Window {
    public static readonly string WindowKey = "###mainWindow";

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
        new OrchestrionTab(),
        new OrnamentTab(),
    };

    private List<ITab> _tabs = new();
    private Stopwatch _stopwatch = new();
    
    public override void OnOpen() {
        base.OnOpen();
        
        // Load in tabs if none are found, somehow.
        if (this._tabs.Count == 0) {
            this._tabs.Add(new OverviewTab(this));
            this._tabs.AddRange(this.DataTabs);
            this._tabs.Add(new SettingsTab());
        }
    }

    public override void Draw() {
        this.WindowName = $"{PluginStrings.CollectorsAnxiety_Title}{WindowKey}";
        var pbs = ImGuiHelpers.GetButtonSize(".");
        
        if (!Injections.ClientState.IsLoggedIn || Injections.ClientState.LocalPlayer == null) {
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
            ImGuiUtil.TextHorizCentered(PluginStrings.CollectorWindow_NoUserLoggedInWarning);
            ImGui.PopStyleColor();
        }

        ImGui.BeginTabBar("mainBar", ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.TabListPopupButton);

        foreach (var tab in this._tabs) {
            if (ImGui.BeginTabItem($"{tab.Name}###{tab.GetType().Name}")) {
                var childSize = ImGui.GetContentRegionAvail();
                var style = ImGui.GetStyle();
                var paddedY = childSize.Y - pbs.Y - 3 * style.ItemSpacing.Y + 2 * style.FramePadding.Y;
                ImGui.BeginChild($"##tabChild-{tab.GetType().Name}", childSize with {Y = paddedY});
                
                this._stopwatch.Start();
                tab.Draw();
                this._stopwatch.Stop();
                
                ImGui.EndChild();

                ImGui.EndTabItem();
            }
        }

        ImGui.EndTabBar();
        
        ImGui.Separator();
        
        ImGui.TextColored(ImGuiColors.DalamudGrey2, $"v{VersionUtil.GetCurrentMajMinBuild()}");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.DalamudOrange, "DEVELOPMENT PREVIEW");
        ImGui.SameLine();
        var stopwatchTime = this._stopwatch.ElapsedTicks / (double) 10000;
        var framerate = ImGui.GetIO().Framerate;
        ImGui.Text($"TAB RENDER: {stopwatchTime:F4}ms ({stopwatchTime / (1000/framerate):F2} frames @ {framerate:F0}fps)");
        this._stopwatch.Reset();
    }
}