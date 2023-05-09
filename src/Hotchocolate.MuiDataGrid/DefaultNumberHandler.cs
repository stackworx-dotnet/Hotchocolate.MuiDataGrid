namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/colDef/gridNumericOperators.ts
public class DefaultNumberHandler<T> : ExpressionBuilderHandler<T>
{
    protected override Expression InternalHandle(ColumnLookupMember member, ExpressionBuilderFlavour flavour, MuiDataGridFilterItemInput filter)
    {
        Expression expression;
        var memberAccessor = member.Expression;
        switch (filter.Operator)
        {
            case "=":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.Equal(memberAccessor, val);
                    break;
                }

            case "!=":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.NotEqual(memberAccessor, val);
                    break;
                }

            case ">":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThan(memberAccessor, val);
                    break;
                }

            case ">=":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThanOrEqual(memberAccessor, val);
                    break;
                }

            case "<":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.LessThan(memberAccessor, val);
                    break;
                }

            case "<=":
                {
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
                    filter.Value.AssertNotNull(filter.Operator);
                    var values = this.GetValueConstantExpressionList(member, filter);
                    var method = GetContainsMethod(member);
                    expression = Expression.Call(values, method, memberAccessor);
                    break;
                }

            default:
                throw new ArgumentException($"Unknown operator: {filter.Operator}");
        }

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return value.AsNumber(member.Type);
    }
}