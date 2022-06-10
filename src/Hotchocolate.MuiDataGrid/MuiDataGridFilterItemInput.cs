namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json.Serialization;
using HotChocolate;
using Stackworx.Hotchocolate.MuiDataGrid.Json;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterItem.ts
public record MuiDataGridFilterItemInput(
    string ColumnField,
    [property: GraphQLType(typeof(MuiValueScalar)), JsonConverter(typeof(MuiValueConverter))]
    MuiValue? Value,
    string OperatorValue,
    string? Id = null);