namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json.Serialization;
using HotChocolate;
using HotChocolate.Types;
using Stackworx.Hotchocolate.MuiDataGrid.Json;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterItem.ts
public record MuiDataGridFilterItemInput(
    string ColumnField,
    [property: GraphQLType(typeof(MuiValueType)), JsonConverter(typeof(MuiValueConverter))]
    MuiValue? Value,
    string OperatorValue,
    [property: GraphQLType(typeof(AnyType))]
    object? Id = null);