using Dalamud.Interface.Internal;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data.Unlockables; 

public class BardingEntry : Unlockable<BuddyEquip> {
    public BardingEntry(BuddyEquip excelRow) : base(excelRow) {
        this.UnlockItem = CollectorsAnxietyPlugin.Instance.UnlockItemCache.GetItemForObject(excelRow);
    }

    public override string Name => this.LuminaEntry.Name.RawString;
    
    public override IDalamudTextureWrap? Icon => 
        CollectorsAnxietyPlugin.Instance.IconManager.GetIconTexture(this.LuminaEntry.IconHead);

    public override Item? UnlockItem { get; }
    
    public override uint SortKey => this.LuminaEntry.Order;

    public override bool IsUnlocked() {
        return CollectorsAnxietyPlugin.Instance.GameState.IsBuddyEquipUnlocked(this.Id);
    }

    public override bool IsValid() {
        return this.LuminaEntry.Order != 0;
    }
}