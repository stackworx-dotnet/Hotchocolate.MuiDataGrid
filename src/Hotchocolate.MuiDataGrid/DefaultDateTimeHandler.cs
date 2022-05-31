namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultDateTimeHandler<T> : IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        Expression expression;
        var memberAccessor = member.Expression;
        switch (filter.OperatorValue)
        {
            case "is":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.Equal(memberAccessor, val);
                    break;
                }

            case "not":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.NotEqual(memberAccessor, val);
                    break;
                }

            case "after":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThan(memberAccessor, val);
                    break;
                }

            case "onOrAfter":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThanOrEqual(memberAccessor, val);
                    break;
                }

            case "before":
                {
                    filter.Value.AssertNotNull(filter.OperatorValue);
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.LessThan(memberAccessor, val);
                    break;
                }

            case "onOrBefore":
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
        return Expression.Constant(DateTime.Parse(filter.Value.AsString()));
    }

    public ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsArray().Select(e => DateTime.Parse(e.AsString())).ToList());
    }
}