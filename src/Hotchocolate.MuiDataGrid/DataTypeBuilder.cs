namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Collections.ObjectModel;

public sealed class DataTypeBuilder<T>
{
    private readonly Dictionary<string, ColumnLookupMember> members = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IExpressionBuilderHandler<T>> handlers = new(StringComparer.Ordinal);

    public DataTypePropertyBuilder<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        return this.Property(expression, GetDefaultFieldName(expression));
    }

    public DataTypePropertyBuilder<T, TProperty> Property<TProperty>(
        Expression<Func<T, TProperty>> expression,
        string fieldName)
    {
        var member = this.CreateLookupMember(expression);

        if (!this.members.TryAdd(fieldName, member))
        {
            throw new ArgumentException($"Column already registered: {fieldName}");
        }

        // Note: this breaks trimming
        var enumHandler = CreateEnumHandlerIfApplicable<TProperty>();
        if (enumHandler != null)
        {
            this.handlers[fieldName] = enumHandler;
        }

        return new DataTypePropertyBuilder<T, TProperty>(this, fieldName);
    }

    public DataTypePropertyBuilder<T, TProperty> Property<TProperty>(
        Expression<Func<T, TProperty>> expression,
        IExpressionBuilderHandler<T> handler)
    {
        var property = this.Property(expression);
        property.SetHandler(handler);
        return property;
    }

    internal void SetFieldName(string currentFieldName, string newFieldName)
    {
        if (string.IsNullOrWhiteSpace(newFieldName))
        {
            throw new ArgumentException("Field name cannot be null or whitespace", nameof(newFieldName));
        }

        if (!this.members.Remove(currentFieldName, out var member))
        {
            throw new ArgumentException($"Unknown field: {currentFieldName}");
        }

        if (!this.members.TryAdd(newFieldName, member))
        {
            throw new ArgumentException($"Column already registered: {newFieldName}");
        }

        if (this.handlers.Remove(currentFieldName, out var handler))
        {
            this.handlers[newFieldName] = handler;
        }
    }

    internal void SetHandler(string fieldName, IExpressionBuilderHandler<T> handler)
    {
        if (!this.members.ContainsKey(fieldName))
        {
            throw new ArgumentException($"Unknown field: {fieldName}");
        }

        this.handlers[fieldName] = handler;
    }

    internal IReadOnlyDictionary<string, ColumnLookupMember> BuildMembers()
    {
        return new ReadOnlyDictionary<string, ColumnLookupMember>(
            new Dictionary<string, ColumnLookupMember>(this.members, StringComparer.Ordinal));
    }

    internal IReadOnlyDictionary<string, IExpressionBuilderHandler<T>> BuildHandlers()
    {
        return new ReadOnlyDictionary<string, IExpressionBuilderHandler<T>>(
            new Dictionary<string, IExpressionBuilderHandler<T>>(this.handlers, StringComparer.Ordinal));
    }

    private static string GetDefaultFieldName<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var memberExpression = ExpressionOperator.GetMemberExpression(expression)
            ?? throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

        return char.ToLowerInvariant(memberExpression.Member.Name[0]) + memberExpression.Member.Name[1..];
    }

    private static IExpressionBuilderHandler<T>? CreateEnumHandlerIfApplicable<TProperty>()
    {
        var enumType = typeof(TProperty).UnwrapNullable();
        if (!enumType.IsEnum)
        {
            return null;
        }

        var handlerType = typeof(DefaultEnumSingleSelectHandler<,>).MakeGenericType(typeof(T), enumType);
        return (IExpressionBuilderHandler<T>?)Activator.CreateInstance(handlerType);
    }

    private ColumnLookupMember CreateLookupMember<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var memberExpression = ExpressionOperator.GetMemberExpression(expression)
            ?? throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

        if (expression.Parameters.Count == 0)
        {
            throw new Exception("Expected 1 parameter and received none");
        }

        var parameterExpression = expression.Parameters[0];
        return new ColumnLookupMember(memberExpression, memberExpression.Member, typeof(TProperty), parameterExpression);
    }
}
