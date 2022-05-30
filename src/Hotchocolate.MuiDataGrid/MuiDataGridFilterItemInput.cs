namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterItem.ts
public record MuiDataGridFilterItemInput(
    string ColumnField,
    [property: GraphQLType(typeof(MuiValueScalar))]
    MuiValue? Value,
    string OperatorValue);