namespace Stackworx.Hotchocolate.MuiDataGrid;

public interface IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter);
}