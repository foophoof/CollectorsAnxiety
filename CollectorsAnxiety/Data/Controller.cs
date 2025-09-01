using System;
using System.Collections.Immutable;
using System.Linq;
using CollectorsAnxiety.Base;
using Dalamud.Plugin.Services;
using Lumina.Excel;

namespace CollectorsAnxiety.Data;

public interface IController
{
    /// <summary>
    /// Get a named tuple of unlocked and total item counts for this controller object.
    ///
    /// This method is combined like this (probably much to the frustration of a few people) in order to prevent
    /// multiple iterations over the item list. The implementation for this method will iterate over the full list
    /// *once*, and count everything at the same time.
    /// </summary>
    /// <param name="respectHidden">Respect the Hidden Items list, defaults to true.</param>
    /// <returns>Returns a named tuple of unlocked and total counts.</returns>
    public (int UnlockedCount, int TotalCount) GetCounts(bool respectHidden = true);

    public bool ParseTainted { get; }
}

public class Controller<TEntry, TSheet> : IController where TEntry : Unlockable<TSheet> where TSheet : struct, IExcelRow<TSheet>
{

    private ImmutableList<TEntry>? _itemCache;

    public required PluginConfig PluginConfig { protected get; init; }
    public required ExcelSheet<TSheet> Sheet { protected get; init; }
    public required IPluginLog PluginLog { protected get; init; }
    public required Func<TSheet, TEntry> EntryFactory { protected get; init; }

    public (int UnlockedCount, int TotalCount) GetCounts(bool respectHidden = true)
    {
        var unlockedCount = 0;
        var totalCount = 0;

        foreach (var item in GetItems())
        {
            if (respectHidden && PluginConfig.IsItemHidden(item))
            {
                continue;
            }

            if (item.IsUnlocked())
            {
                unlockedCount += 1;
            }

            totalCount += 1;
        }

        return (unlockedCount, totalCount);
    }

    public bool ParseTainted => PluginConfig.HiddenItems
        .ContainsKey(typeof(TEntry).Name);

    public virtual ImmutableList<TEntry> GetItems(bool useCache = true)
    {
        if (_itemCache != null && useCache)
        {
            return _itemCache;
        }

        PluginLog.Debug($"Cache miss or invalidated getting items for {typeof(TEntry).Name}, regenerating list");

        _itemCache = Sheet
            .Select(EntryFactory)
            .Where(entry => entry.IsValid())
            .OrderBy(entry => entry.SortKey)
            .ToImmutableList();

        return _itemCache;
    }
}
