namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterItem.ts
public interface IMuiDataGridFilterItemInput
{
    /*
    // We likely don't need this field
    // TODO: number | string
    // public string? Id { get; set; }
    */

    public string ColumnField { get; }

    public MuiValue? Value { get; }

    public string OperatorValue { get; }
}