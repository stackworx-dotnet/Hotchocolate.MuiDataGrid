namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Utilities;

public abstract class ExpressionBuilderHandler<T> : IExpressionBuilderHandler<T>
{
    public Expression<Func<T, bool>> Handle(
        ColumnLookupMember member,
        ExpressionBuilderFlavour flavour,
        MuiDataGridFilterItemInput filter)
    {
        var expression = this.InternalHandle(member, flavour, filter);
        return Expression.Lambda<Func<T, bool>>(expression, member.ParameterExpression);
    }

    protected static MethodInfo GetContainsMethod(ColumnLookupMember member)
    {
        var generic = typeof(ICollection<>);
        var constructed = generic.MakeGenericType(member.Member.GetReturnType());
        return constructed.GetMethod("Contains") ?? throw new InvalidOperationException();
    }

    protected ConstantExpression GetValueConstantExpression(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(this.ParseValue(member, filter.Value), member.Expression.Type);
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

    protected abstract Expression InternalHandle(ColumnLookupMember member, ExpressionBuilderFlavour flavour, MuiDataGridFilterItemInput filter);

    protected abstract dynamic ParseValue(ColumnLookupMember member, MuiValue value);

    protected Expression WrapWithNullCheck(MemberExpression memberExpression, Expression expression)
    {
        var nullCheck = Expression.NotEqual(memberExpression, Expression.Constant(null));
        return Expression.AndAlso(nullCheck, expression);
    }

    private static dynamic CreateGenericList(Type t)
    {
        var generic = typeof(List<>);
        var constructed = generic.MakeGenericType(t);
        return Activator.CreateInstance(constructed) ?? throw new InvalidOperationException();
    }
}
