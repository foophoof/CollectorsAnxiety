using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CollectorsAnxiety.Data;
using CollectorsAnxiety.Util;
using Dalamud.Interface.Colors;
using ImGuiNET;
using Lumina.Excel;

namespace CollectorsAnxiety.UI;

public interface ITab : IDisposable {
    public string Name { get; }

    public void Draw();
}

public interface IDataTab : ITab {
    public bool ShowInOverview { get; }
    
    public IController GetController();
}

public enum FilterMode {
    ShowAll,
    ShowUnlocked,
    ShowLocked,
    ShowHiddenOnly,
}

public class BaseTab<TEntry, TSheet> : IDataTab where TEntry : DataEntry<TSheet> where TSheet : ExcelRow {
    public virtual string Name => "Tab";
    public virtual bool ShowInOverview => true;
    
    protected Controller<TEntry, TSheet> Controller;
    
    protected virtual (string Name, ImGuiTableColumnFlags Flags, int Width)[] TableColumns => new[] {
        ("Unlocked", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHeaderLabel, 32), 
        ("Icon", ImGuiTableColumnFlags.WidthFixed, 48), 
        ("Number", ImGuiTableColumnFlags.WidthFixed, 48),
        ("Name", ImGuiTableColumnFlags.None, -1),
    };
    
    private FilterMode _displayFilter = FilterMode.ShowAll;
    private bool _showHidden;

    private ImGuiListClipperPtr _clipperPtr;

    public BaseTab() {
        this.Controller = new Controller<TEntry, TSheet>();

        unsafe {
            this._clipperPtr = new ImGuiListClipperPtr(ImGuiNative.ImGuiListClipper_ImGuiListClipper());
        }
    }

    public void Dispose() {
        this._clipperPtr.Destroy();
    }

    public IController GetController() {
        return this.Controller;
    }

    public virtual void Draw() {
        var displayMode = (int) this._displayFilter;
        var filterLabels = new List<string> {"Show All", "Show Complete", "Show Incomplete"};
        if (this._showHidden) filterLabels.Add("Show Hidden Only");
        if (ImGui.Combo("###filter", ref displayMode, filterLabels.ToArray(), filterLabels.Count)) {
            this._displayFilter = (FilterMode) displayMode;
        }
        
        ImGui.SameLine();
        ImGui.Dummy(new Vector2(10, 0));
        ImGui.SameLine();
        if (ImGui.Checkbox("Show Hidden", ref this._showHidden)) {
            if (this._displayFilter == FilterMode.ShowHiddenOnly)
                this._displayFilter = FilterMode.ShowAll;
        }
        
        // load in items early so we can cache and operate on the subset.
        var totalVisibleItems = 0;
        var unlockedVisibleItems = 0;
        var itemsToRender = new List<(bool Unlocked, bool Hidden, TEntry Item)>();

        foreach (var item in this.Controller.GetItems()) {
            var isItemUnlocked = item.IsUnlocked();
            var isItemHidden = CollectorsAnxietyPlugin.Instance.Configuration.IsItemHidden(item);

            if (!isItemHidden) totalVisibleItems += 1;
            if (isItemUnlocked && !isItemHidden) unlockedVisibleItems += 1;
            
            if (isItemHidden && !this._showHidden) continue;
            
            switch (this._displayFilter) {
                case FilterMode.ShowLocked when isItemUnlocked:
                case FilterMode.ShowUnlocked when !isItemUnlocked:
                case FilterMode.ShowHiddenOnly when !isItemHidden:
                    continue;
            }
            
            itemsToRender.Add((isItemUnlocked, isItemHidden, item));
        }

        ImGuiUtil.CompletionProgressBar(unlockedVisibleItems, totalVisibleItems);

        if (ImGui.BeginTable($"##TabTable_{this.Name}", this.TableColumns.Length, ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY)) {
            ImGui.TableSetupScrollFreeze(0, 1);
            foreach (var col in this.TableColumns) {
                ImGui.TableSetupColumn(col.Name, col.Flags, col.Width);
            }
            ImGui.TableHeadersRow();

            this._clipperPtr.Begin(itemsToRender.Count, 52);
            while (this._clipperPtr.Step()) {
                for (var index = this._clipperPtr.DisplayStart; index < this._clipperPtr.DisplayEnd; index++) {
                    var icr = itemsToRender[index];
                    var item = icr.Item;
                    ImGui.TableNextRow(ImGuiTableRowFlags.None, 52);

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Dummy(new Vector2(0, 8));
                    ImGui.Checkbox("", ref icr.Unlocked);
                    if (ImGui.BeginPopupContextItem($"context_{this.Name}#{icr.Item.Id}")) {
                        if (ImGui.MenuItem("Hide Item", "", icr.Hidden)) {
                            if (!icr.Hidden) {
                                CollectorsAnxietyPlugin.Instance.Configuration.HideItem(icr.Item);
                            } else {
                                CollectorsAnxietyPlugin.Instance.Configuration.UnhideItem(icr.Item);
                            }
                        }
                    
                        ImGui.EndPopup();
                    }

                    ImGui.TableSetColumnIndex(1);
                    var icon = item.Icon;
                    if (icon != null)
                        ImGui.Image(icon.ImGuiHandle, new Vector2(48));

                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text($"#{item.Id}");

                    ImGui.TableSetColumnIndex(3);
                    ImGui.Text(item.Name);
                    if (icr.Hidden) {
                        ImGui.SameLine();
                        ImGui.TextColored(ImGuiColors.DalamudGrey3, "(Hidden)");
                    }
                }
            }

            ImGui.EndTable();
        }
    }
}