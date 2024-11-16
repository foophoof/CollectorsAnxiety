using CollectorsAnxiety.Base;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Util;

public static class LuminaUtil {
    public static bool IsMarketBoardEligible(this Item? item) {
        if (item == null) {
            return false;
        }
        
        return item.Value.RowId != 0 && item.Value.ItemSearchCategory.RowId != 0;
    }
}
