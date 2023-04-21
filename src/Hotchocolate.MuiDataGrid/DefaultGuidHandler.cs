namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultGuidHandler<T> : ExpressionBuilderHandler<T>
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

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return Guid.Parse(value.AsString());
    }
}