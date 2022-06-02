namespace Stackworx.Hotchocolate.MuiDataGrid;

public abstract class BaseColumnLookup<T> : IColumnLookup<T>
{
    public ColumnLookupMember Lookup(string column)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var e = this.InternalLookup(parameter, column);

        if (e == null)
        {
            throw new ArgumentException($"Unhandled Column: {column}");
        }

        return e;
    }

    public bool CanHandle(string column)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var lookup = this.InternalLookup(parameter, column);
        return lookup != null;
    }

    protected ColumnLookupMember GetMemberExpression<TProperty>(
        ParameterExpression parameter,
        Expression<Func<T, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is MemberExpression member)
        {
            if (member.Member is PropertyInfo propInfo)
            {
                var expressionParameter = Expression.Property(parameter, propInfo.Name);
                return new ColumnLookupMember(expressionParameter, typeof(TProperty));
            }

            throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
        }

        throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
    }

    protected abstract ColumnLookupMember? InternalLookup(ParameterExpression parameter, string column);
}