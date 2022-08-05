using System;
using System.Collections.Generic;
using CollectorsAnxiety.Data;
using Dalamud.Configuration;
using Dalamud.Logging;

namespace CollectorsAnxiety.Base; 

public class PluginConfig : IPluginConfiguration {
    public int Version { get; set; } = 1;
    
    public Dictionary<string, HashSet<uint>> HiddenItems = new();
    
    public void Save() {
        Injections.PluginInterface.SavePluginConfig(this);
    }

    public bool IsItemHidden<T>(T entry) where T : IDataEntry {
        var entryKey = typeof(T).Name;
        
        return this.HiddenItems.ContainsKey(entryKey) && this.HiddenItems[entryKey].Contains(entry.Id);
    }

    public void HideItem<T>(T entry) where T : IDataEntry {
        var entryKey = typeof(T).Name;

        if (!this.HiddenItems.TryGetValue(entryKey, out var thisSet))
            thisSet = this.HiddenItems[entryKey] = new HashSet<uint>();

        thisSet.Add(entry.Id);
        this.Save();
    }

    public void UnhideItem<T>(T entry) where T : IDataEntry {
        var entryKey = typeof(T).Name;

        if (!this.HiddenItems.TryGetValue(entryKey, out var thisSet))
            return;

        if (thisSet.Contains(entry.Id)) thisSet.Remove(entry.Id);

        if (thisSet.Count == 0) this.HiddenItems.Remove(entryKey);
            
        this.Save();
    }
}