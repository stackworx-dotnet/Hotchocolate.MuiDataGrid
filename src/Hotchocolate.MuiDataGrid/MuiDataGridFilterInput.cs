namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterModel.ts
public record MuiDataGridFilterInput
{
    public IList<MuiDataGridFilterItemInput> Items { get; set; } = new List<MuiDataGridFilterItemInput>();

    public MuiDataGridLogicOperator? LogicOperator { get; set; }

    [property: GraphQLType(typeof(MuiValueType))]
    public MuiValue? QuickFilterValues { get; set; }

    public MuiDataGridLogicOperator? QuickFilterLogicOperator { get; set; }
}