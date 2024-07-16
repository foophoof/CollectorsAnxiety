using System.Collections.Generic;
using System.Collections.Immutable;
using CollectorsAnxiety.Base;
using CollectorsAnxiety.Game;
using CollectorsAnxiety.Resources.Localization;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class HairstyleEntry : Unlockable<CharaMakeCustomize> {
    public HairstyleEntry(CharaMakeCustomize excelRow) : base(excelRow) {
        this.Id = this.LuminaEntry.Data;
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForUnlockLink(excelRow.Data);
    }

    public bool WearableByMale;
    public bool WearableByFemale;

    public readonly HashSet<GameCompat.PlayerRace> WearableByMaleRaceIDs = new();
    public readonly HashSet<GameCompat.PlayerRace> WearableByFemaleRaceIDs = new();

    public override uint Id { get; }

    public override string Name => this.GetName();

    public override uint SortKey => this.LuminaEntry.Data;

    public override uint? IconId => this.LuminaEntry.Icon;

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
        var styleIdDict = new Dictionary<uint, HairstyleEntry>();

        foreach (var styleRow in Injections.DataManager.GetExcelSheet<CharaMakeCustomize>()!) {
            if (styleRow.Data == 0) continue;

            if (!itemDict.TryGetValue(styleRow.Data, out var styleEntry)) {
                styleEntry = new HairstyleEntry(styleRow);
                if (!styleEntry.IsValid()) continue;

                itemDict[styleRow.Data] = styleEntry;
            }

            styleIdDict[styleRow.RowId] = styleEntry;
        }

        var hairMakeType = Injections.DataManager.GetExcelSheet<HairMakeType>()!;
        foreach (var rowParser in hairMakeType.GetRowParsers()) {
            var race = (GameCompat.PlayerRace) rowParser.ReadColumn<int>(0);
            var gender = (GameCompat.PlayerGender) rowParser.ReadColumn<sbyte>(2);

            for (var i = 0; i < 100; i++) {
                var hairStyle = rowParser.ReadColumn<uint>(66 + i * 9);

                if (hairStyle == 0)
                    continue;

                if (!styleIdDict.TryGetValue(hairStyle, out var styleEntry))
                    continue;

                switch (gender) {
                    case GameCompat.PlayerGender.Female:
                        styleEntry.WearableByFemale = true;
                        styleEntry.WearableByFemaleRaceIDs.Add(race);
                        break;

                    case GameCompat.PlayerGender.Male:
                        styleEntry.WearableByMale = true;
                        styleEntry.WearableByMaleRaceIDs.Add(race);
                        break;
                }
            }

            for (var i = 0; i < 100; i++) {
                var facepaintStyle = rowParser.ReadColumn<uint>(73 + i * 9);
                if (!styleIdDict.TryGetValue(facepaintStyle, out var styleEntry))
                    continue;

                switch (gender) {
                    case GameCompat.PlayerGender.Female:
                        styleEntry.WearableByFemale = true;
                        styleEntry.WearableByFemaleRaceIDs.Add(race);
                        break;

                    case GameCompat.PlayerGender.Male:
                        styleEntry.WearableByMale = true;
                        styleEntry.WearableByMaleRaceIDs.Add(race);
                        break;
                }
            }
        }

        this._itemCache = itemDict.Values.ToImmutableList();
        return this._itemCache;
    }
}
