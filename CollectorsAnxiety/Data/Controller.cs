using System;
using System.Collections.Immutable;
using System.Linq;
using CollectorsAnxiety.Base;
using Dalamud.Logging;
using Lumina.Excel;

namespace CollectorsAnxiety.Data;

public interface IController {
    public (int UnlockedCount, int TotalCount) GetCounts(bool respectHidden = true);
    
    public bool ParseTainted { get; }
}

public class Controller<TEntry, TSheet> : IController where TEntry : DataEntry<TSheet> where TSheet : ExcelRow {

    private ImmutableList<TEntry>? _itemCache;

    private static ExcelSheet<TSheet> GetSheet() {
        return Injections.DataManager.GetExcelSheet<TSheet>()!;
    }

    public (int UnlockedCount, int TotalCount) GetCounts(bool respectHidden = true) {
        var unlockedCount = 0;
        var totalCount = 0;

        foreach (var item in this.GetItems()) {
            if (respectHidden && CollectorsAnxietyPlugin.Instance.Configuration.IsItemHidden(item))
                continue;

            if (item.IsUnlocked())
                unlockedCount += 1;

            totalCount += 1;
        }

        return (unlockedCount, totalCount);
    }

    public bool ParseTainted => CollectorsAnxietyPlugin.Instance.Configuration.HiddenItems
        .ContainsKey(typeof(TEntry).Name);

    public virtual ImmutableList<TEntry> GetItems(bool useCache = true) {
        if (this._itemCache != null && useCache) {
            return this._itemCache;
        }

        PluginLog.Debug($"Cache miss or invalidated getting items for {typeof(TEntry).Name}, regenerating list");
        
        this._itemCache = GetSheet()
            .Select(row => (TEntry) Activator.CreateInstance(typeof(TEntry), row)!)
            .Where(entry => entry.IsValid())
            .ToImmutableList();

        return this._itemCache;
    }
}