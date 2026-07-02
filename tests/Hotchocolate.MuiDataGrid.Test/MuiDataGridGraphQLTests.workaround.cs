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
                // HC16 coerces enums by their GraphQL name; passing the raw CLR enum serialises
                // to a Number the enum type rejects. Send the mapped name ("or"/"and").
                "logicOperator", input.LogicOperator == MuiDataGridLogicOperator.Or ? "or" : "and"
            },
        }.ToImmutableDictionary();
    }
}