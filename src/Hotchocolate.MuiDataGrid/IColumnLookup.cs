namespace Stackworx.Hotchocolate.MuiDataGrid;

public interface IColumnLookup<T>
{
    public ColumnLookupMember Lookup(string column);

    public bool CanHandle(string column);
}