namespace Stackworx.Hotchocolate.MudDataGrid;

using System.Linq.Expressions;
using Stackworx.Hotchocolate.MuiDataGrid;

public static class ExpressionBuilderExtensions
{
    public static IQueryable<T> Sort<T>(
        this ExpressionBuilder<T> builder,
        IQueryable<T> query,
        MudDataGridFilterInput filters)
    {
        var muiSorts = MudToMuiDataGridAdapter.MapSort(filters.SortDefinitions);
        return query.Sort(builder, muiSorts);
    }

    public static Expression<Func<T, bool>> Filter<T>(this ExpressionBuilder<T> builder, MudDataGridFilterInput filters)
    {
        var muiFilters = MudToMuiDataGridAdapter.MapFilters(filters.FilterDefinitions);

        if (muiFilters.Items.Count == 0)
        {
            return _ => true;
        }

        return builder.Filter(muiFilters);
    }
}