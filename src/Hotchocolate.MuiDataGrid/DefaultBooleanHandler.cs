namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/colDef/gridBooleanOperators.ts
public class DefaultBooleanHandler<T> : IExpressionBuilderHandler<T>
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
        return Expression.Constant(filter.Value.AsBoolean());
    }

    public ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        throw new NotImplementedException();
    }
}