namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/colDef/gridDateOperators.ts
public class DefaultDateTimeHandler<T> : ExpressionBuilderHandler<T>
{
    protected override Expression InternalHandle(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
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

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return DateTime.Parse(value.AsString());
    }
}