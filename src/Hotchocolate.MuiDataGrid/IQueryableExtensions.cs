namespace Stackworx.Hotchocolate.MuiDataGrid;

public static class IQueryableExtensions
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> q, ExpressionBuilder<T> builder, IList<MuiDataGridSortItem> items)
    {
        return builder.Sort(q, items);
    }
}