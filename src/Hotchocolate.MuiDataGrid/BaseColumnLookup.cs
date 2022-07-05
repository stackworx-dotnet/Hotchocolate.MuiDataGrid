namespace Stackworx.Hotchocolate.MuiDataGrid;

public abstract class BaseColumnLookup<T> : IColumnLookup<T>
{
    public ColumnLookupMember Lookup(string column)
    {
        var e = this.InternalLookup(column);

        if (e == null)
        {
            throw new ArgumentException($"Unhandled Column: {column}");
        }

        return e;
    }

    public bool CanHandle(string column)
    {
        var lookup = this.InternalLookup(column);
        return lookup != null;
    }

    protected ColumnLookupMember GetMemberExpression<TProperty>(
        Expression<Func<T, TProperty>> expression)
    {
        var memberExpression = ExpressionOperator.GetMemberExpression(expression);
        if (memberExpression == null)
        {
            throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");
        }

        if (expression.Parameters.Count == 0)
        {
            throw new Exception("Expected 1 parameter and received none");
        }

        var parameterExpression = expression.Parameters[0];
        return new ColumnLookupMember(memberExpression, typeof(TProperty), parameterExpression);
    }

    protected abstract ColumnLookupMember? InternalLookup(string column);
}
