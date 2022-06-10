namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json.Serialization;
using Stackworx.Hotchocolate.MuiDataGrid.Json;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/models/gridFilterModel.ts
public record MuiDataGridFilterInput
{
    public IList<MuiDataGridFilterItemInput> Items { get; set; } = new List<MuiDataGridFilterItemInput>();

    [JsonConverter(typeof(MuiDataGridLinkOperatorConverter))]
    public MuiDataGridLinkOperator? LinkOperator { get; set; }
}