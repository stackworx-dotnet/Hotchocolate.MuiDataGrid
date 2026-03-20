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

    public static MuiDataGridFilterInput MapFilters(IList<MudDataGridFilterDefinitionInput> filterDefinitions, IReadOnlyDictionary<string, string>? customOperators = null)
    {
        var items = filterDefinitions
            .Select(f => MapFilter(f, customOperators))
            .ToList();

        return new MuiDataGridFilterInput
        {
            Items = items,
            LogicOperator = MuiDataGridLogicOperator.And,
        };
    }

    public static IList<MuiDataGridSortItem> MapSort(IList<MudDataGridSortDefinitionInput> sortDefinitions)
    {
        return sortDefinitions
            .Select(s => new MuiDataGridSortItem(s.Field, s.Descending ? MuiGridSortDirection.Desc : MuiGridSortDirection.Asc))
            .ToList();
    }

    private static MuiDataGridFilterItemInput MapFilter(MudDataGridFilterDefinitionInput input, IReadOnlyDictionary<string, string>? customOperators)
    {
        var normalizedOperator = NormalizeOperator(input.Operator, input.Field, customOperators);
        return new MuiDataGridFilterItemInput(input.Field, input.Value, normalizedOperator, input.Id, "mud");
    }

    // MUI Data grid Operartors
    // https://github.com/mui/mui-x/tree/v8.28.0/packages/x-data-grid/src/colDef
    private static string NormalizeOperator(
        string @operator,
        string field,
        IReadOnlyDictionary<string, string>? customOperators)
    {
        if (customOperators is not null)
        {
            if (customOperators.TryGetValue(@operator, out var normalizedOperator))
            {
                return normalizedOperator;
            }
        }

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
}