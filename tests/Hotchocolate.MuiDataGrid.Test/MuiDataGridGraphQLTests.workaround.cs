namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Collections.Immutable;

public partial class MuiDataGridGraphQLTests
{
    // Hack to workaround - https://github.com/ChilliCream/hotchocolate/issues/4911
    private ImmutableDictionary<string, object?> ConvertFilterInput(MuiDataGridFilterInput input)
    {
        return new Dictionary<string, object?>
        {
            {
                "items", input.Items
            },
            {
                "linkOperator", input.LinkOperator
            },
        }.ToImmutableDictionary();
    }
}