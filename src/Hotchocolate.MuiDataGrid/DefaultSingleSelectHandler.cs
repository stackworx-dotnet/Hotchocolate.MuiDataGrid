namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultSingleSelectHandler<T> : IExpressionBuilderHandler<T>
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
                    expression = Expression.Not(Expression.Equal(memberAccessor, val));
                    break;
                }

            case "isAnyOf":
                {
                    var generic = typeof(ICollection<>);
                    var constructed = generic.MakeGenericType(member.Type);
                    var method = constructed.GetMethod("Contains")!;
                    expression = Expression.Call(this.GetValueConstantExpressionList(member, filter), method, memberAccessor);
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

    public virtual ConstantExpression GetValueConstantExpression(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsString());
    }

    public virtual ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsArray().Select(e => e.AsString()).ToList());
    }
}