namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/colDef/gridStringOperators.ts
public class DefaultStringHandler<T> : IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        Expression expression;
        var memberAccessor = member.Expression;
        switch (filter.OperatorValue)
        {
            case "equals":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.Equal(memberAccessor, val);
                    break;
                }

            case "contains":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    expression = Expression.Call(memberAccessor, method, val);
                    break;
                }

            case "startsWith":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!;
                    expression = Expression.Call(memberAccessor, method, val);
                    break;
                }

            case "endsWith":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!;
                    expression = Expression.Call(memberAccessor, endsWithMethod, val);
                    break;
                }

            case "isEmpty":
                {
                    MethodInfo method =
                        typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) })!;
                    expression = Expression.Call(method, memberAccessor);
                    break;
                }

            case "isNotEmpty":
                {
                    MethodInfo isNullOrEmptyMethod =
                        typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) })!;
                    expression = Expression.Not(Expression.Call(isNullOrEmptyMethod, memberAccessor));
                    break;
                }

            case "isAnyOf":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var values = this.GetValueConstantExpressionList(member, filter);
                    MethodInfo method =
                        typeof(ICollection<string>).GetMethod("Contains")!;
                    expression = Expression.Call(values, method, memberAccessor);
                    break;
                }

            default:
                throw new Exception($"Unknown operator: {filter.OperatorValue}");
        }

        if (memberAccessor.Expression is ParameterExpression p)
        {
            return Expression.Lambda<Func<T, bool>>(expression, new[] { p });
        }

        throw new ArgumentException($"Expected ParameterExpression. Got: {memberAccessor.Expression}");
    }

    public ConstantExpression GetValueConstantExpression(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsString());
    }

    public ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsArray().Select(e => e.AsString()).ToList());
    }
}