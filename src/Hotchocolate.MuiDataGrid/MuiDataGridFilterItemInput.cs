namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate;
using HotChocolate.Types;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterItem.ts
public record MuiDataGridFilterItemInput(
    string Field,
    [property: GraphQLType(typeof(MuiValueType))]
    MuiValue? Value,
    string Operator,
    [property: GraphQLType(typeof(AnyType))]
    object? Id = null,
    string? FromInput = null);