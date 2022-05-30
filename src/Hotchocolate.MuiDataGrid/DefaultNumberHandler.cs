namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/colDef/gridNumericOperators.ts
public class DefaultNumberHandler<T> : IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        Expression expression;
        var memberAccessor = member.Expression;
        switch (filter.OperatorValue)
        {
            case "=":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.Equal(memberAccessor, val);
                    break;
                }

            case "!=":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.NotEqual(memberAccessor, val);
                    break;
                }

            case ">":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThan(memberAccessor, val);
                    break;
                }

            case ">=":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThanOrEqual(memberAccessor, val);
                    break;
                }

            case "<":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.LessThan(memberAccessor, val);
                    break;
                }

            case "<=":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.LessThanOrEqual(memberAccessor, val);
                    break;
                }

            case "isEmpty":
                {
                    if (memberAccessor.Type.IsNullable())
                    {
                        expression = Expression.Equal(Expression.Constant(null), memberAccessor);
                    }
                    else
                    {
                        expression = Expression.Constant(true);
                    }

                    break;
                }

            case "isNotEmpty":
                {
                    expression = Expression.NotEqual(memberAccessor, Expression.Constant(null));
                    break;
                }

            case "isAnyOf":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var values = this.GetValueConstantExpressionList(member, filter);
                    MethodInfo method = GetContainsMethod(member);
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
        return Expression.Constant(filter.Value.AsNumber(member.Type));
    }

    public ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        // TODO: error if any nulls
        var elements = filter.Value.AsArray();
        switch (member.Type)
        {
            // TODO: exclude nulls
            case var x when x == typeof(int):
                return Expression.Constant(elements.Select(e => e.AsInt()).ToList());
            case var x when x == typeof(double):
                return Expression.Constant(elements.Select(e => e.AsDouble()).ToList());
            case var x when x == typeof(float):
                return Expression.Constant(elements.Select(e => e.AsFloat()).ToList());
            case var x when x == typeof(short):
                return Expression.Constant(elements.Select(e => e.AsShort()).ToList());
            default:
                throw new ArgumentException($"Invalid type: {member.Type}");
        }
    }

    private static MethodInfo GetContainsMethod(ColumnLookupMember member)
    {
        switch (member.Type)
        {
            case var x when x == typeof(int):
                return typeof(ICollection<int>).GetMethod("Contains")!;
            case var x when x == typeof(double):
                return typeof(ICollection<double>).GetMethod("Contains")!;
            case var x when x == typeof(float):
                return typeof(ICollection<float>).GetMethod("Contains")!;
            case var x when x == typeof(short):
                return typeof(ICollection<short>).GetMethod("Contains")!;
            default:
                throw new ArgumentException($"Invalid type: {member.Type}");
        }
    }
}