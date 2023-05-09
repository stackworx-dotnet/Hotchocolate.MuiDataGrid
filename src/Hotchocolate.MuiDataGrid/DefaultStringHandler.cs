namespace Stackworx.Hotchocolate.MuiDataGrid;

// https://github.com/mui/mui-x/blob/master/packages/grid/x-data-grid/src/colDef/gridStringOperators.ts
public class DefaultStringHandler<T> : ExpressionBuilderHandler<T>
{
    protected override Expression InternalHandle(ColumnLookupMember member, ExpressionBuilderFlavour flavour, MuiDataGridFilterItemInput filter)
    {
        Expression expression;
        var memberAccessor = member.Expression;
        switch (filter.Operator)
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
                    var method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    var callExpression = Expression.Call(memberAccessor, method, val);

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

            case "startsWith":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!;
                    var callExpression = Expression.Call(memberAccessor, method, val);

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

            case "endsWith":
                {
                    var val = this.GetValueConstantExpression(member, filter);
                    var method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!;
                    var callExpression = Expression.Call(memberAccessor, method, val);

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
                throw new ArgumentException($"Unknown operator: {filter.Operator}");
        }

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return value.AsString();
    }
}