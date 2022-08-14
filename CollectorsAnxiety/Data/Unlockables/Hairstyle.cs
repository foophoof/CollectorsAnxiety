using System.Collections.Generic;
using System.Collections.Immutable;
using CollectorsAnxiety.Resources.Localization;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables;

public class HairstyleEntry : DataEntry<CharaMakeCustomize> {
    public HairstyleEntry(CharaMakeCustomize excelRow) : base(excelRow) {
        this.Id = this.LuminaEntry.Data;
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForUnlockLink(excelRow.Data);
    }

    public override uint Id { get; }

    public override string Name => this.GetName();
    
    public override TextureWrap? Icon =>
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture((int) this.LuminaEntry.Icon);

    public override Item? UnlockItem { get; }

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsUnlockLinkUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.IsPurchasable;
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
        
        var seenIds = new HashSet<uint>();
        var items = new List<HairstyleEntry>();
        
        foreach (var style in base.GetItems(useCache)) {
            if (seenIds.Contains(style.Id))
                continue;

            items.Add(style);
            seenIds.Add(style.Id);
        }

        this._itemCache = items.ToImmutableList();
        return this._itemCache;
    }
}