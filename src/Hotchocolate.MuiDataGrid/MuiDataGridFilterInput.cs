namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json.Serialization;
using HotChocolate;
using Stackworx.Hotchocolate.MuiDataGrid.Json;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterModel.ts
public record MuiDataGridFilterInput
{
    public IList<MuiDataGridFilterItemInput> Items { get; set; } = new List<MuiDataGridFilterItemInput>();

    [JsonConverter(typeof(MuiDataGridLinkOperatorConverter))]
    public MuiDataGridLogicOperator? LogicOperator { get; set; }

    [property: GraphQLType(typeof(MuiValueType))]
    [property: JsonConverter(typeof(MuiValueConverter))]
    public MuiValue? QuickFilterValues { get; set; }

    [JsonConverter(typeof(MuiDataGridLinkOperatorConverter))]
    public MuiDataGridLogicOperator? QuickFilterLogicOperator { get; set; }
}