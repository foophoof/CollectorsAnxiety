using System.Collections.Generic;
using System.Collections.Immutable;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.Resources.Localization;
using Dalamud.Interface.Internal;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class HairstyleEntry : Unlockable<CharaMakeCustomize> {
    public HairstyleEntry(CharaMakeCustomize excelRow) : base(excelRow) {
        this.Id = this.LuminaEntry.Data;
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForUnlockLink(excelRow.Data);
    }

    public bool WearableByMale = false;
    public bool WearableByFemale = false;
    
    public readonly HashSet<GameCompat.PlayerRace> WearableByRaceIDs = new();

    public override uint Id { get; }

    public override string Name => this.GetName();

    public override uint SortKey => this.LuminaEntry.Data;

    public override IDalamudTextureWrap? Icon =>
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture((int) this.LuminaEntry.Icon);

    public override Item? UnlockItem { get; }

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsUnlockLinkUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.IsPurchasable && this.LuminaEntry.Icon != 0 && !string.IsNullOrWhiteSpace(this.GetName());
    }

    private string GetName() {
        return this.LuminaEntry.Data switch {
            228 => "Eternal Bonding",
            _ => this.LuminaEntry.HintItem.Value?.Name.RawString ?? UIStrings.ErrorHandling_Unknown
        };
    }
}

public class HairstyleController : Controller<HairstyleEntry, CharaMakeCustomize> {
    private ImmutableList<HairstyleEntry>? _itemCache;

    public override ImmutableList<HairstyleEntry> GetItems(bool useCache = true) {
        if (this._itemCache != null && useCache)
            return this._itemCache;

        var itemDict = new Dictionary<uint, HairstyleEntry>();

        foreach (var styleRow in Injections.DataManager.GetExcelSheet<CharaMakeCustomize>()!) {
            if (styleRow.Data == 0) continue;

            if (!itemDict.TryGetValue(styleRow.Data, out var styleEntry)) {
                styleEntry = new HairstyleEntry(styleRow);
                if (!styleEntry.IsValid()) continue;
                
                itemDict[styleRow.Data] = styleEntry;
            }

            if (styleRow.RowId < 2000) {
                // Hairstyles
                var categorizationId = styleRow.RowId / 100;

                styleEntry.WearableByMale |= (categorizationId % 2) == 0;
                styleEntry.WearableByFemale |= (categorizationId % 2) == 1;

                // Hyurs are a pain in my side because midlanders/highlanders each have their own block, which nobody
                // else does. So, we'll redirect block 0 (midlander hyur) to block 1 to bring everything into sync.
                var raceId = categorizationId / 2;
                if (raceId == 0) raceId = 1;
                styleEntry.WearableByRaceIDs.Add((GameCompat.PlayerRace) raceId);
            } else {
                // Facepaint
                var categorizationId = (styleRow.RowId - 2000) / 50;
                
                styleEntry.WearableByMale |= (categorizationId % 2) == 0;
                styleEntry.WearableByFemale |= (categorizationId % 2) == 1;
                styleEntry.WearableByRaceIDs.Add((GameCompat.PlayerRace) (categorizationId / 4) + 1);
            }

        }

        this._itemCache = itemDict.Values.ToImmutableList();
        return this._itemCache;
    }
}