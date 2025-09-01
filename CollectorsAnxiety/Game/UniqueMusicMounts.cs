using System.Collections.Generic;
using System.Linq;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Game;

public class UniqueMusicMounts
{
    public UniqueMusicMounts(ExcelSheet<Mount> mountSheet)
    {
        UniqueMounts = mountSheet
            .GroupBy(n => n.RideBGM.RowId)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToHashSet();
    }

    private HashSet<Mount> UniqueMounts { get; }

    public bool HasUniqueMusic(Mount mount)
    {
        return UniqueMounts.Contains(mount);
    }
}
