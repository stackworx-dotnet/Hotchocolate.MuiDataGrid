namespace Stackworx.Hotchocolate.MudDataGrid;

using HotChocolate;
using HotChocolate.Types;
using Stackworx.Hotchocolate.MuiDataGrid;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

public record MudDataGridFilterDefinitionInput(
    string Field,
    string Operator,
    [property: GraphQLType(typeof(MuiValueType))] MuiValue? Value,
    [property: GraphQLType(typeof(AnyType))] object? Id = null);
