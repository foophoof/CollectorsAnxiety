using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Util; 

public static class LuminaUtil {
    public static bool IsMarketBoardEligible(this Item? item) {
        return item != null && item.ItemSearchCategory.Row != 0;
    }
}