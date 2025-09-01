using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace CollectorsAnxiety.Data;

public interface IUnlockable
{
    public uint Id { get; }
}

public abstract class Unlockable<T> : IUnlockable where T : struct, IExcelRow<T>
{
    internal readonly T LuminaEntry;

    protected Unlockable(T excelRow)
    {
        LuminaEntry = excelRow;
        Id = LuminaEntry.RowId;
    }

    public virtual uint Id { get; }

    public virtual Item? UnlockItem => null;

    public abstract string Name { get; }
    public abstract uint? IconId { get; }

    public virtual uint SortKey => Id;

    public abstract bool IsUnlocked();

    public abstract bool IsValid();
}
