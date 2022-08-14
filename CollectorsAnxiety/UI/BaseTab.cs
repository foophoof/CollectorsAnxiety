using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CollectorsAnxiety.Data;
using CollectorsAnxiety.Resources.Localization;
using CollectorsAnxiety.Util;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using ImGuiNET;
using ImGuiScene;
using Lumina.Excel;

namespace CollectorsAnxiety.UI;

public interface ITab {
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

public class TableColumn {
    public string Name { get; }
    public ImGuiTableColumnFlags Flags { get; }
    public int Width { get; }

    public TableColumn(string name, ImGuiTableColumnFlags flags = ImGuiTableColumnFlags.None, int width = -1) {
        this.Name = name;
        this.Flags = flags;
        this.Width = width;
    }
}

public class BaseTab<TEntry, TSheet> : IDataTab where TEntry : DataEntry<TSheet> where TSheet : ExcelRow {
    private const int IconSize = 48;
    
    public virtual string Name => "Tab";
    public virtual bool ShowInOverview => true;

    protected Controller<TEntry, TSheet> Controller;

    private readonly TableColumn[] _tableColumns = {
        new("Unlocked", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoHeaderLabel, 32),
        new("Icon", ImGuiTableColumnFlags.WidthFixed, 48),
        new("Number", ImGuiTableColumnFlags.WidthFixed, 48),
        new("Name")
    };

    protected virtual TableColumn[]? ExtraColumns => null;

    private FilterMode _displayFilter = FilterMode.ShowAll;
    private bool _showHidden;

    private ImGuiListClipperPtr _clipperPtr;

    protected BaseTab() {
        this.Controller = new Controller<TEntry, TSheet>();

        unsafe {
            this._clipperPtr = new ImGuiListClipperPtr(ImGuiNative.ImGuiListClipper_ImGuiListClipper());
        }
    }

    ~BaseTab() {
        this._clipperPtr.Destroy();
    }

    public IController GetController() {
        return this.Controller;
    }

    public virtual void Draw() {
        var hideSpoilers = CollectorsAnxietyPlugin.Instance.Configuration.HideSpoilers;
        TextureWrap? spoilerIcon = null;
        if (hideSpoilers)
            spoilerIcon = CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(000786);

        var displayMode = (int) this._displayFilter;
        var filterLabels = new List<string> {
            UIStrings.BaseTab_FilterShowAll, 
            UIStrings.BaseTab_FilterShowComplete,
            UIStrings.BaseTab_FilterShowIncomplete
        };
        if (this._showHidden) filterLabels.Add(UIStrings.BaseTab_FilterHiddenOnly);
        if (ImGui.Combo("###filter", ref displayMode, filterLabels.ToArray(), filterLabels.Count)) {
            this._displayFilter = (FilterMode) displayMode;
        }

        ImGui.SameLine();
        ImGui.Dummy(new Vector2(10, 0));
        ImGui.SameLine();
        if (ImGui.Checkbox(UIStrings.BaseTab_FilterShowHidden, ref this._showHidden)) {
            if (this._displayFilter == FilterMode.ShowHiddenOnly)
                this._displayFilter = FilterMode.ShowAll;
        }

        ImGuiComponents.HelpMarker(UIStrings.BaseTab_HiddenHelp);

        // load in items early so we can cache and operate on the subset.
        var totalVisibleItems = 0;
        var unlockedVisibleItems = 0;
        var itemsToRender = new List<(bool Unlocked, bool Hidden, TEntry Item)>();

        // This is a bit of a logical "wtf", but it's unfortunately the fastest way I can think of to handle this.
        // Instead of using controller methods to get counts and all, we can just iterate over the list once here and
        // handle all parsing/processing in one easy place. This handles hidden item counts, filtering, and basically
        // everything else once a frame. Notably, this is the ***only*** iteration over the full list during this tab's
        // render.
        foreach (var item in this.Controller.GetItems()) {
            var isItemUnlocked = item.IsUnlocked();
            var isItemHidden = CollectorsAnxietyPlugin.Instance.Configuration.IsItemHidden(item);

            if ((!isItemHidden || this._showHidden)) totalVisibleItems += 1;
            if (isItemUnlocked && (!isItemHidden || this._showHidden)) unlockedVisibleItems += 1;

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

        var applicableColumns = this._tableColumns;
        if (this.ExtraColumns != null) {
            applicableColumns = applicableColumns.Concat(this.ExtraColumns).ToArray();
        }

        if (ImGui.BeginTable($"##TabTable_{this.GetType().Name}", applicableColumns.Length,
                ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollY)) {
            ImGui.TableSetupScrollFreeze(0, 1);
            foreach (var col in applicableColumns) {
                ImGui.TableSetupColumn(col.Name, col.Flags, col.Width);
            }

            ImGui.TableHeadersRow();

            // Clipper needs to have an idea of the cell size. This will always be the icon size (48) plus double
            // padding for table entries. 
            var cellHeight = IconSize + 2 * ImGui.GetStyle().CellPadding.Y;
            this._clipperPtr.Begin(itemsToRender.Count, cellHeight);
            while (this._clipperPtr.Step()) {
                for (var index = this._clipperPtr.DisplayStart; index < this._clipperPtr.DisplayEnd; index++) {
                    var (unlocked, hidden, item) = itemsToRender[index];
                    ImGui.TableNextRow(ImGuiTableRowFlags.None, IconSize);

                    var censorItem = (!unlocked && hideSpoilers);

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Dummy(new Vector2(0, 8));
                    ImGui.Checkbox("", ref unlocked);
                    if (ImGui.BeginPopupContextItem($"context_{this.GetType().Name}#{item.Id}")) {
                        if (ImGui.MenuItem(UIStrings.BaseTab_HideItem, "", hidden)) {
                            if (!hidden) {
                                CollectorsAnxietyPlugin.Instance.Configuration.HideItem(item);
                            } else {
                                CollectorsAnxietyPlugin.Instance.Configuration.UnhideItem(item);
                            }
                        }

                        ImGui.EndPopup();
                    }

                    ImGui.TableSetColumnIndex(1);
                    var icon = censorItem ? spoilerIcon : item.Icon;
                    if (icon != null)
                        ImGui.Image(icon.ImGuiHandle, new Vector2(IconSize));

                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text($"#{item.Id}");

                    ImGui.TableSetColumnIndex(3);
                    ImGui.Text(censorItem ? UIStrings.BaseTab_SpoilerMask : item.Name);
                    if (hidden) {
                        ImGui.SameLine();
                        ImGui.TextColored(ImGuiColors.DalamudGrey3, UIStrings.BaseTab_HiddenTag);
                    }

                    if (!censorItem) {
                        this.DrawEntryIcons(item);
                        
                        var tagline = this.GetTagline(item);
                        if (tagline != null) {
                            ImGui.TextColored(ImGuiColors.DalamudGrey2, tagline);
                        }
                    }

                    this.DrawExtraColumns(item);
                }
            }

            ImGui.EndTable();
        }
    }

    protected virtual void DrawExtraColumns(TEntry entry) { }

    protected virtual void DrawEntryIcons(TEntry entry) {
        var unlockItem = entry.UnlockItem;
            
        if (unlockItem.IsMarketboardEligible())
            ImGuiUtil.HoverMarker(FontAwesomeIcon.Coins, UIStrings.BaseTab_MarketBoardPurchaseable);
    }

    protected virtual string? GetTagline(TEntry entry) => null;
}