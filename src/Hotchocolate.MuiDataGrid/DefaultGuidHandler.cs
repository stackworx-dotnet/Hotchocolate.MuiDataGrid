namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultGuidHandler<T> : ExpressionBuilderHandler<T>
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

            case "isEmpty":
            {
                var method = typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) })!;
                var callExpression = Expression.Call(method, memberAccessor);

                if (member.IsNullable && flavour == ExpressionBuilderFlavour.IN_MEMORY)
                {
                    expression = this.WrapWithNullCheck(member.Expression, callExpression);
                }
                else
                {
                    expression = callExpression;
                }

                break;
            }

            case "isNotEmpty":
            {
                var method = typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) })!;
                var callExpression = Expression.Not(Expression.Call(method, memberAccessor));

                if (member.IsNullable && flavour == ExpressionBuilderFlavour.IN_MEMORY)
                {
                    expression = this.WrapWithNullCheck(member.Expression, callExpression);
                }
                else
                {
                    expression = callExpression;
                }

                break;
            }

            case "isAnyOf":
            {
                var values = this.GetValueConstantExpressionList(member, filter);
                var method = typeof(ICollection<string>).GetMethod("Contains")!;
                var callExpression = Expression.Call(values, method, memberAccessor);

                if (member.IsNullable && flavour == ExpressionBuilderFlavour.IN_MEMORY)
                {
                    expression = this.WrapWithNullCheck(member.Expression, callExpression);
                }
                else
                {
                    expression = callExpression;
                }

                break;
            }

            default:
                throw new Exception($"Unknown operator: {filter.OperatorValue}");
        }

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return Guid.Parse(value.AsString());
    }
}