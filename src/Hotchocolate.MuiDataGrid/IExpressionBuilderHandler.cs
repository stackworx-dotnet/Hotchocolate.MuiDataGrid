namespace Stackworx.Hotchocolate.MuiDataGrid;

public interface IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter);

    public ConstantExpression GetValueConstantExpression(ColumnLookupMember member, MuiDataGridFilterItemInput filter);

    public ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter);
}