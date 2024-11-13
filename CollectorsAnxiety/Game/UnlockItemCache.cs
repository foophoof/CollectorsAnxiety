using System.Collections.Generic;
using System.Linq;
using CollectorsAnxiety.Base;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Game;

public class UnlockItemCache {
    // Ref: E8 ?? ?? ?? ?? 84 C0 75 A6 32 C0

    /// Workaround for certain items that are found, but for whatever reason can't actually be obtained.
    private static readonly uint[] BlockedItemIds = {
        24225 // Unlock book for Tomestone emote, unused.
    };

    private readonly Dictionary<(string UnlockableType, uint UnlockableId), Item> _cache = new();

    public UnlockItemCache() {
        this.LoadCache();
    }

    private void LoadCache(bool force = false) {
        if (this._cache.Count != 0 && !force) return;

        var itemSheet = Injections.DataManager.Excel.GetSheet<Item>();

        foreach (var item in itemSheet) {
            if (BlockedItemIds.Contains(item.RowId)) continue;

            var itemAction = item.ItemAction.Value;

            switch (itemAction.Type) {
                case 0xA49: // Unlock Link (Emote, Hairstyle)
                    this._cache[("UnlockLink", itemAction.Data[0])] = item;
                    break;

                case 0x355: // Minions
                    this._cache[(nameof(Companion), itemAction.Data[0])] = item;
                    break;

                case 0x3F5: // Bardings
                    this._cache[(nameof(BuddyEquip), itemAction.Data[0])] = item;
                    break;

                case 0x52A: // Mounts
                    this._cache[(nameof(Mount), itemAction.Data[0])] = item;
                    break;

                case 0xD1D: // Triple Triad Cards
                    this._cache[(nameof(TripleTriadCard), item.AdditionalData.RowId)] = item;
                    break;

                case 0x4E76: // Ornaments
                    this._cache[(nameof(Ornament), itemAction.Data[0])] = item;
                    break;

                case 0x625F: // Orchestrion Rolls
                    this._cache[(nameof(Orchestrion), item.AdditionalData.RowId)] = item;
                    break;

                case 37312: // Facewear
                    this._cache[(nameof(Glasses), item.AdditionalData.RowId)] = item;
                    break;

                default:
                    continue;
            }
        }

        Injections.PluginLog.Debug($"Loaded {this._cache.Count} unlockable items into cache.");
    }

    public Item? GetItemForUnlockLink(uint unlockLink) {
        var found = this._cache.TryGetValue(("UnlockLink", unlockLink), out var item);
        if (!found) {
            return null;
        }
        return item;
    }

    public Item? GetItemForObject<T>(T unlockable) where T : struct, IExcelRow<T> {
        var found = this._cache.TryGetValue((unlockable.GetType().Name, unlockable.RowId), out var item);
        if (!found) {
            return null;
        }
        return item;
    }
}
