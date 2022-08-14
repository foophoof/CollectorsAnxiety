using System.Collections.Generic;
using System.Linq;
using CollectorsAnxiety.Base;
using Dalamud.Logging;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Game; 

public class UnlockItemCache {
    // Ref: E8 ?? ?? ?? ?? 84 C0 75 A6 32 C0
    
    private readonly Dictionary<(string UnlockableType, uint UnlockableId), Item> _cache = new();

    public UnlockItemCache() {
        this.LoadCache();
    }

    private void LoadCache(bool force = false) {
        if (this._cache.Count != 0 && !force) return;
        
        var itemSheet = Injections.DataManager.Excel.GetSheet<Item>()!;
        
        foreach (var item in itemSheet) {
            var itemAction = item.ItemAction.Value;
            if (itemAction == null) continue;

            switch (itemAction.Type) {
                case 0xA49:  // Unlock Link (Emote, Hairstyle)
                    this._cache[("UnlockLink", itemAction.Data[0])] = item;
                    break;

                case 0x355:  // Minions
                    this._cache[(nameof(Companion), itemAction.Data[0])] = item;
                    break;
                
                case 0x3F5:  // Bardings
                    this._cache[(nameof(BuddyEquip), itemAction.Data[0])] = item;
                    break;
                
                case 0x52A:  // Minion
                    this._cache[(nameof(Mount), itemAction.Data[0])] = item;
                    break;
                
                case 0xD1D:  // Triple Triad
                    this._cache[(nameof(TripleTriadCard), item.AdditionalData)] = item;
                    break;
                
                case 0x4E76: // Ornament
                    this._cache[(nameof(Ornament), itemAction.Data[0])] = item;
                    break;
                
                case 0x625f: // Orchestion
                    this._cache[(nameof(Orchestrion), item.AdditionalData)] = item;
                    break;

                default:
                    continue;
            }
        }
        
        PluginLog.Debug($"Loaded {this._cache.Count} unlockable items into cache.");
    }

    public Item? GetItemForUnlockLink(uint unlockLink) {
        this._cache.TryGetValue(("UnlockLink", unlockLink), out var item);
        return item;
    }

    public Item? GetItemForObject(ExcelRow unlockable) {
        this._cache.TryGetValue((unlockable.GetType().Name, unlockable.RowId), out var item);
        return item;
    }
}