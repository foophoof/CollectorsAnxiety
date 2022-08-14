using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Util; 

public static class LuminaUtil {
    public static bool IsMarketboardEligible(this Item? item) {
        return item != null && item.ItemSearchCategory.Row != 0;
    }
}