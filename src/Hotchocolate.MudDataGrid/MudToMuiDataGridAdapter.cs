namespace Stackworx.Hotchocolate.MudDataGrid;

using HotChocolate;
using Stackworx.Hotchocolate.MuiDataGrid;

public static class MudToMuiDataGridAdapter
{
    public static MudDataGridAdapterResult Map(MudDataGridFilterInput? input)
    {
        if (input is null)
        {
            return new MudDataGridAdapterResult(null, null);
        }

        return new MudDataGridAdapterResult(MapFilters(input.FilterDefinitions), MapSort(input.SortDefinitions));
    }

    public static IList<MuiDataGridSortItem> MapSort(IList<MudDataGridSortDefinitionInput> sortDefinitions)
    {
        return sortDefinitions
            .Select(s => new MuiDataGridSortItem(s.Field, s.Descending ? MuiGridSortDirection.Desc : MuiGridSortDirection.Asc))
            .ToList();
    }

    private static MuiDataGridFilterItemInput MapFilter(MudDataGridFilterDefinitionInput input)
    {
        var normalizedOperator = NormalizeOperator(input.Operator, input.Field);
        return new MuiDataGridFilterItemInput(input.Field, input.Value, normalizedOperator, input.Id, "mud");
    }

    private static string NormalizeOperator(string @operator, string field)
    {
        return @operator.Trim().ToLowerInvariant() switch
        {
            "contains" => "contains",
            "starts with" => "startsWith",
            "ends with" => "endsWith",
            "equal" => "equals",
            "equals" => "equals",
            "=" => "=",
            "!=" => "!=",
            ">" => ">",
            ">=" => ">=",
            "<" => "<",
            "<=" => "<=",
            "is" => "is",
            "is not" => "not",
            "not" => "not",
            "empty" => "isEmpty",
            "not empty" => "isNotEmpty",
            "any of" => "isAnyOf",
            "is any of" => "isAnyOf",
            _ => throw CreateOperatorError(@operator, field),
        };
    }

    private static GraphQLException CreateOperatorError(string @operator, string field)
    {
        var error = ErrorBuilder.New()
            .SetCode("MUD_OPERATOR_NOT_SUPPORTED")
            .SetMessage($"MudDataGrid operator '{@operator}' is not supported for field '{field}'.")
            .SetExtension("field", field)
            .SetExtension("operator", @operator)
            .Build();

        return new GraphQLException(error);
    }

    public static MuiDataGridFilterInput MapFilters(IList<MudDataGridFilterDefinitionInput> filterDefinitions)
    {
        var items = filterDefinitions
            .Select(MapFilter)
            .ToList();

        return new MuiDataGridFilterInput
        {
            Items = items,
            LogicOperator = MuiDataGridLogicOperator.And,
        };
    }
}
