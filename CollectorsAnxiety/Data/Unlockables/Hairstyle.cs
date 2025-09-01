using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using CollectorsAnxiety.Game;
using Dalamud.Plugin.Services;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class HairstyleEntry : Unlockable<CharaMakeCustomize>
{
    public HairstyleEntry(CharaMakeCustomize excelRow, UnlockItemCache unlockItemCache) : base(excelRow)
    {
        this.Id = this.LuminaEntry.UnlockLink;
        this.UnlockItem = unlockItemCache.GetItemForUnlockLink(excelRow.UnlockLink);
    }

    public bool WearableByMale;
    public bool WearableByFemale;

    public readonly HashSet<GameCompat.PlayerRace> WearableByMaleRaceIDs = new();
    public readonly HashSet<GameCompat.PlayerRace> WearableByFemaleRaceIDs = new();

    public override uint Id { get; }

    public override string Name => this.GetName();

    public override uint SortKey => this.LuminaEntry.UnlockLink;

    public override uint? IconId => this.LuminaEntry.Icon;

    public override Item? UnlockItem { get; }

    public override bool IsUnlocked()
    {
        return GameState.IsUnlockLinkUnlocked(this.Id);
    }

    public override bool IsValid()
    {
        return this.LuminaEntry.IsPurchasable && this.LuminaEntry.Icon != 0 && !string.IsNullOrWhiteSpace(this.GetName());
    }

    private string GetName()
    {
        return this.LuminaEntry.UnlockLink switch
        {
            228 => "Eternal Bonding",
            _ => this.LuminaEntry.HintItem.ValueNullable?.Name.ExtractText() ?? "Unknown"
        };
    }
}

public class HairstyleController : Controller<HairstyleEntry, CharaMakeCustomize>
{
    public required Func<CharaMakeCustomize, HairstyleEntry> HairstyleEntryFactory { protected get; init; }
    public required IDataManager DataManager { protected get; init; }

    private ImmutableList<HairstyleEntry>? _itemCache;

    public override ImmutableList<HairstyleEntry> GetItems(bool useCache = true)
    {
        if (this._itemCache != null && useCache)
            return this._itemCache;

        var itemDict = new Dictionary<uint, HairstyleEntry>();
        var styleIdDict = new Dictionary<uint, HairstyleEntry>();

        foreach (var styleRow in this.Sheet)
        {
            if (styleRow.UnlockLink == 0) continue;

            if (!itemDict.TryGetValue(styleRow.UnlockLink, out var styleEntry))
            {
                styleEntry = this.HairstyleEntryFactory(styleRow);
                if (!styleEntry.IsValid()) continue;

                itemDict[styleRow.UnlockLink] = styleEntry;
            }

            styleIdDict[styleRow.RowId] = styleEntry;
        }

        foreach (var row in this.DataManager.GetExcelSheet<RawRow>(name: "HairMakeType"))
        {
            var race = (GameCompat.PlayerRace)row.ReadInt32Column(0);
            var gender = (GameCompat.PlayerGender)row.ReadInt8Column(2);

            var numHairstyles = row.ReadUInt8Column(30);
            for (var i = 0; i < numHairstyles; i++)
            {
                var hairStyle = row.ReadUInt32Column(66 + i * 9);

                if (hairStyle == 0)
                    continue;

                if (!styleIdDict.TryGetValue(hairStyle, out var hairstyleEntry))
                    continue;

                switch (gender)
                {
                    case GameCompat.PlayerGender.Female:
                        hairstyleEntry.WearableByFemale = true;
                        hairstyleEntry.WearableByFemaleRaceIDs.Add(race);
                        break;

                    case GameCompat.PlayerGender.Male:
                        hairstyleEntry.WearableByMale = true;
                        hairstyleEntry.WearableByMaleRaceIDs.Add(race);
                        break;
                }
            }

            var numFacepaints = row.ReadUInt8Column(37);
            for (var i = 0; i < numFacepaints; i++)
            {
                var facepaintStyle = row.ReadUInt32Column(73 + i * 9);
                if (!styleIdDict.TryGetValue(facepaintStyle, out var facepaintEntry))
                    continue;

                switch (gender)
                {
                    case GameCompat.PlayerGender.Female:
                        facepaintEntry.WearableByFemale = true;
                        facepaintEntry.WearableByFemaleRaceIDs.Add(race);
                        break;

                    case GameCompat.PlayerGender.Male:
                        facepaintEntry.WearableByMale = true;
                        facepaintEntry.WearableByMaleRaceIDs.Add(race);
                        break;
                }
            }
        }

        this._itemCache = itemDict.Values.ToImmutableList();
        return this._itemCache;
    }
}
