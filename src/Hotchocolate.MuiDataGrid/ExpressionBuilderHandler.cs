namespace Stackworx.Hotchocolate.MuiDataGrid;

public abstract class ExpressionBuilderHandler<T> : IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        var memberAccessor = member.Expression;
        var expression = this.InternalHandle(member, filter);
        if (memberAccessor.Expression is ParameterExpression p)
        {
            return Expression.Lambda<Func<T, bool>>(expression, p);
        }

        throw new ArgumentException($"Expected ParameterExpression. Got: {memberAccessor.Expression}");
    }

    protected ConstantExpression GetValueConstantExpression(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(this.ParseValue(member, filter.Value));
    }

    protected ConstantExpression GetValueConstantExpressionList(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        var list = CreateGenericList(member.Type);
        foreach (var value in filter.Value.AsArray())
        {
            list.Add(this.ParseValue(member, value));
        }

        return Expression.Constant(list);
    }

    protected static MethodInfo GetContainsMethod(ColumnLookupMember member)
    {
        var generic = typeof(ICollection<>);
        var constructed = generic.MakeGenericType(member.Type);
        return constructed.GetMethod("Contains") ?? throw new InvalidOperationException();
    }

    protected abstract Expression InternalHandle(ColumnLookupMember member, MuiDataGridFilterItemInput filter);

    protected abstract dynamic ParseValue(ColumnLookupMember member, MuiValue value);

    private static dynamic CreateGenericList(Type t)
    {
        var generic = typeof(List<>);
        var constructed = generic.MakeGenericType(t);
        return Activator.CreateInstance(constructed) ?? throw new InvalidOperationException();
    }
}