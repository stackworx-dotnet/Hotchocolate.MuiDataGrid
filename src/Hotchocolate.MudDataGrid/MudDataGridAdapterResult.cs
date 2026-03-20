namespace Stackworx.Hotchocolate.MudDataGrid;

using Stackworx.Hotchocolate.MuiDataGrid;

public record MudDataGridAdapterResult(
    MuiDataGridFilterInput? Filters,
    IList<MuiDataGridSortItem>? Sorting);