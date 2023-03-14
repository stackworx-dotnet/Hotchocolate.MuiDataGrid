namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultPredicateHandler<T> : ExpressionBuilderHandler<T>
{
    protected override Expression InternalHandle(ColumnLookupMember member, ExpressionBuilderFlavour flavour, MuiDataGridFilterItemInput filter)
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
                    var method = typeof(string).GetMethod(
                        "Contains",
                        new[]
                        {
                        typeof(string),
                        })!;
                    expression = Expression.Call(memberAccessor, method, val);
                    break;
                }

            case "startsWith":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    var method = typeof(string).GetMethod(
                        "StartsWith",
                        new[]
                        {
                        typeof(string),
                        })!;
                    expression = Expression.Call(memberAccessor, method, val);
                    break;
                }

            case "endsWith":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    var endsWithMethod = typeof(string).GetMethod(
                        "EndsWith",
                        new[]
                        {
                        typeof(string),
                        })!;
                    expression = Expression.Call(memberAccessor, endsWithMethod, val);
                    break;
                }

            case "isEmpty":
                {
                    var method =
                        typeof(string).GetMethod(
                            "IsNullOrEmpty",
                            new[]
                            {
                            typeof(string),
                            })!;
                    expression = Expression.Call(method, memberAccessor);
                    break;
                }

            case "isNotEmpty":
                {
                    var isNullOrEmptyMethod =
                        typeof(string).GetMethod(
                            "IsNullOrEmpty",
                            new[]
                            {
                            typeof(string),
                            })!;
                    expression = Expression.Not(Expression.Call(isNullOrEmptyMethod, memberAccessor));
                    break;
                }

            case "isAnyOf":
                {
                    var values = this.GetValueConstantExpressionList(member, filter);
                    var method = typeof(ICollection<string>).GetMethod("Contains")!;
                    expression = Expression.Call(values, method, memberAccessor);
                    break;
                }

            default:
                throw new ArgumentException($"Unknown operator: {filter.OperatorValue}");
        }

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return value.AsString();
    }
}
