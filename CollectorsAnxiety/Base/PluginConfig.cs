using System.Collections.Generic;
using System.Linq;
using CollectorsAnxiety.Data;
using Dalamud.Configuration;

namespace CollectorsAnxiety.Base;

public class PluginConfig : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public readonly Dictionary<string, HashSet<uint>> HiddenItems = new();

    public bool HideSpoilers { get; set; } = true;
    public bool CountHiddenItemsInOverview { get; set; } = false;

    public bool IsItemHidden<T>(T entry) where T : IUnlockable
    {
        var entryKey = typeof(T).Name;

        return HiddenItems.ContainsKey(entryKey) && HiddenItems[entryKey].Contains(entry.Id);
    }

    public void HideItem<T>(T entry) where T : IUnlockable
    {
        var entryKey = typeof(T).Name;

        if (!HiddenItems.TryGetValue(entryKey, out var thisSet))
        {
            thisSet = HiddenItems[entryKey] = new HashSet<uint>();
        }

        thisSet.Add(entry.Id);
    }

    public void UnhideItem<T>(T entry) where T : IUnlockable
    {
        var entryKey = typeof(T).Name;

        if (!HiddenItems.TryGetValue(entryKey, out var thisSet))
        {
            return;
        }

        if (thisSet.Contains(entry.Id))
        {
            thisSet.Remove(entry.Id);
        }

        if (thisSet.Count == 0)
        {
            HiddenItems.Remove(entryKey);
        }
    }

    internal bool PerformCleanups()
    {
        var wasCleanupDone = false;

        // Clean up empty hidden items that (somehow) managed to get in to config.
        var copy = HiddenItems.ToHashSet();
        foreach (var (key, value) in copy)
        {
            if (value.Count != 0)
            {
                continue;
            }

            HiddenItems.Remove(key);
            wasCleanupDone = true;
        }

        return wasCleanupDone;
    }
}
