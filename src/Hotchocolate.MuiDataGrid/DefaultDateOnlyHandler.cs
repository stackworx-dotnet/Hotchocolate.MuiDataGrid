namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultDateOnlyHandler<T> : ExpressionBuilderHandler<T>
{
    protected override Expression InternalHandle(ColumnLookupMember member, ExpressionBuilderFlavour flavour, MuiDataGridFilterItemInput filter)
    {
        Expression expression;
        var memberAccessor = member.Expression;
        switch (filter.OperatorValue)
        {
            case "is":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.Equal(memberAccessor, val);
                    break;
                }

            case "not":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.NotEqual(memberAccessor, val);
                    break;
                }

            case "after":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThan(memberAccessor, val);
                    break;
                }

            case "onOrAfter":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.GreaterThanOrEqual(memberAccessor, val);
                    break;
                }

            case "before":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    expression = Expression.LessThan(memberAccessor, val);
                    break;
                }

            case "onOrBefore":
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

            default:
                throw new Exception($"Unknown operator: {filter.OperatorValue}");
        }

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return DateOnly.Parse(value.AsString());
    }
}