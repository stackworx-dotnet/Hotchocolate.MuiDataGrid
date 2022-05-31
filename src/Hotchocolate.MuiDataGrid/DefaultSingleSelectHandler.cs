namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultSingleSelectHandler<T> : ExpressionBuilderHandler<T>
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
                    var left = memberAccessor;
                    var right = val;
                    var t1 = left.Type;
                    var t2 = right.Type;
                    var b1 = left.Type == typeof(object);
                    var b2 = left.Type.IsEnum;
                    var b3 = left.Type == typeof(object);
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

        return expression;
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        return value.AsString();
    }
}