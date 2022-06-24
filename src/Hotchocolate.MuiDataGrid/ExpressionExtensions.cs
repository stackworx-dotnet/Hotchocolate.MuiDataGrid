namespace Stackworx.Hotchocolate.MuiDataGrid;

internal static class ExpressionExtensions
{
    public static string GetPropertyPath<TObj, T, TRet>(
        this TObj obj,
        Expression<Func<T, TRet>> expr)
    {
        return ExpressionOperator.GetPropertyPath(expr);
    }
}
