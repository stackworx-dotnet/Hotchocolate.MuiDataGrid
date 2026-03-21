namespace Stackworx.Hotchocolate.MudDataGrid;

using System.Linq.Expressions;
using Stackworx.Hotchocolate.MuiDataGrid;

public static class DataTypeExtensions
{
    public static IQueryable<T> Sort<T>(
        this DataType<T> builder,
        IQueryable<T> query,
        MudDataGridFilterInput filters)
    {
        var muiSorts = MudToMuiDataGridAdapter.MapSort(filters.SortDefinitions);
        return builder.Sort(query, muiSorts);
    }

    public static Expression<Func<T, bool>> Filter<T>(this DataType<T> builder, MudDataGridFilterInput filters, IReadOnlyDictionary<string, string>? customOperators = null)
    {
        var muiFilters = MudToMuiDataGridAdapter.MapFilters(filters.FilterDefinitions, customOperators);

        if (muiFilters.Items.Count == 0)
        {
            return _ => true;
        }

        return builder.Filter(muiFilters);
    }
}