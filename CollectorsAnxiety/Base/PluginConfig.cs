using System.Collections.Generic;
using System.Linq;
using CollectorsAnxiety.Data;
using Dalamud.Configuration;

namespace CollectorsAnxiety.Base;

public class PluginConfig : IPluginConfiguration {
    public int Version { get; set; } = 1;

    public readonly Dictionary<string, HashSet<uint>> HiddenItems = new();

    public bool HideSpoilers { get; set; } = true;
    public bool CountHiddenItemsInOverview { get; set; } = false;

    public PluginConfig() {
        this.PerformCleanups();
    }

    public void Save() {
        Injections.PluginInterface.SavePluginConfig(this);
    }

    public bool IsItemHidden<T>(T entry) where T : IUnlockable {
        var entryKey = typeof(T).Name;

        return this.HiddenItems.ContainsKey(entryKey) && this.HiddenItems[entryKey].Contains(entry.Id);
    }

    public void HideItem<T>(T entry) where T : IUnlockable {
        var entryKey = typeof(T).Name;

        if (!this.HiddenItems.TryGetValue(entryKey, out var thisSet))
            thisSet = this.HiddenItems[entryKey] = new HashSet<uint>();

        thisSet.Add(entry.Id);
        this.Save();
    }

    public void UnhideItem<T>(T entry) where T : IUnlockable {
        var entryKey = typeof(T).Name;

        if (!this.HiddenItems.TryGetValue(entryKey, out var thisSet))
            return;

        if (thisSet.Contains(entry.Id)) thisSet.Remove(entry.Id);

        if (thisSet.Count == 0) this.HiddenItems.Remove(entryKey);

        this.Save();
    }

    private void PerformCleanups() {
        var wasCleanupDone = false;

        // Clean up empty hidden items that (somehow) managed to get in to config.
        var copy = this.HiddenItems.ToHashSet();
        foreach (var (key, value) in copy) {
            if (value.Count != 0) continue;

            this.HiddenItems.Remove(key);
            wasCleanupDone = true;
        }

        if (wasCleanupDone)
            this.Save();
    }
}
