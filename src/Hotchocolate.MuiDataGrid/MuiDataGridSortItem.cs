namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json.Serialization;
using Stackworx.Hotchocolate.MuiDataGrid.Json;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridSortModel.ts#L21
public record MuiDataGridSortItem(
    string Field,
    [property: JsonConverter(typeof(MuiGridSortDirectionConverter))] MuiGridSortDirection Sort);