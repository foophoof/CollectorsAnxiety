using ImGuiScene;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace CollectorsAnxiety.Data;

public interface IDataEntry {
    public uint Id { get; }
}

public abstract class DataEntry<T> : IDataEntry where T : ExcelRow {
    protected readonly T LuminaEntry;

    protected DataEntry(T excelRow) {
        this.LuminaEntry = excelRow;
        this.Id = this.LuminaEntry.RowId;
    }

    public virtual uint Id { get; }

    public virtual Item? UnlockItem => null;

    public abstract string Name { get; }
    public abstract TextureWrap? Icon { get; }

    public virtual uint SortKey => this.Id;

    public abstract bool IsUnlocked();

    public abstract bool IsValid();
}