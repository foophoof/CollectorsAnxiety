using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data;

public interface IUnlockable {
    public uint Id { get; }
}

public abstract class Unlockable<T> : IUnlockable where T : ExcelRow {
    internal readonly T LuminaEntry;

    protected Unlockable(T excelRow) {
        this.LuminaEntry = excelRow;
        this.Id = this.LuminaEntry.RowId;
    }

    public virtual uint Id { get; }

    public virtual Item? UnlockItem => null;

    public abstract string Name { get; }
    public abstract uint? IconId { get; }

    public virtual uint SortKey => this.Id;

    public abstract bool IsUnlocked();

    public abstract bool IsValid();
}