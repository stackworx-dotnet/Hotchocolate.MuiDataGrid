namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Language;

public class ExpressionBuilder<T>
{
    private readonly Dictionary<string, IExpressionBuilderHandler<T>> handlers = new();
    private readonly IColumnLookup<T> columnLookup;
    private readonly ExpressionBuilderFlavour flavour;

    private readonly IExpressionBuilderHandler<T> defaultBooleanHandler = new DefaultBooleanHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultStringHandler = new DefaultStringHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultNumberHandler = new DefaultNumberHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultSingleSelectHandler = new DefaultSingleSelectHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultDateTimeHandler = new DefaultDateTimeHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultDateOnlyHandler = new DefaultDateOnlyHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultGuidHandler = new DefaultGuidHandler<T>();

    public ExpressionBuilder(IColumnLookup<T> columnLookup, ExpressionBuilderFlavour flavour = ExpressionBuilderFlavour.EFCORE)
    {
        this.columnLookup = columnLookup;
        this.flavour = flavour;
    }

    public void AddHandler(string columnField, IExpressionBuilderHandler<T> handler)
    {
        // Validate column
        if (!this.columnLookup.CanHandle(columnField))
        {
            throw new ArgumentException($"{this.columnLookup} cannot handle column {columnField}");
        }

        if (!this.handlers.ContainsKey(columnField))
        {
            this.handlers[columnField] = handler;
        }
        else
        {
            throw new ArgumentException(
                $"Handler for column: {columnField} already registered");
        }
    }

    public IQueryable<T> Sort(IQueryable<T> query, IList<MuiDataGridSortItem> items)
    {
        var q = query;
        foreach (var item in items)
        {
            q = this.Sort(q, item);
        }

        return q;
    }

    public Expression<Func<T, bool>> Filter(MuiDataGridFilterInput filters)
    {
        var predicates = new List<Expression<Func<T, bool>>>();

        foreach (var item in filters.Items)
        {
            predicates.Add(this.Build(item));
        }

        switch (predicates.Count)
        {
            // short circuit
            case 0:
                return _ => true;
            // skip combine
            case 1:
                return predicates[0];
            default:
                {
                    var first = predicates.Pop();
                    return filters.LogicOperator == MuiDataGridLogicOperator.Or
                        ? predicates.Aggregate(first, Or)
                        : predicates.Aggregate(first, And);
                }
        }
    }

    private static Expression<Func<T, bool>> Or(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    private static Expression<Func<T, bool>> And(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }

    // MemberExpression memberAccessor,
    private Expression<Func<T, bool>> Build(MuiDataGridFilterItemInput filter)
    {
        var memberAccessor = this.columnLookup.Lookup(filter.Field);

        if (this.handlers.TryGetValue(filter.Field, out var handler))
        {
            return handler.Handle(memberAccessor, this.flavour, filter);
        }

        var t = memberAccessor.Type.UnwrapNullable();

        switch (t)
        {
            case var x when x == typeof(int):
                return this.defaultNumberHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(double):
                return this.defaultNumberHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(float):
                return this.defaultNumberHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(short):
                return this.defaultNumberHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(decimal):
                return this.defaultNumberHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(string):
                return this.defaultStringHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(bool):
                return this.defaultBooleanHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(DateTime):
                return this.defaultDateTimeHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(DateTimeOffset):
                return this.defaultDateTimeHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(DateOnly):
                return this.defaultDateOnlyHandler.Handle(memberAccessor, this.flavour, filter);
            case var x when x == typeof(Guid):
                return this.defaultGuidHandler.Handle(memberAccessor, this.flavour, filter);
            default:
                throw new ArgumentException($"Unexpected Member Type {t}");
        }
    }

    private IQueryable<T> Sort(IQueryable<T> query, MuiDataGridSortItem item)
    {
        var member = this.columnLookup.Lookup(item.Field);
        var memberAccessor = member.Expression;

        dynamic expression;
        if (memberAccessor.Expression is ParameterExpression p)
        {
            expression = Expression.Lambda(memberAccessor, p);
        }
        else
        {
            throw new ArgumentException($"Expected ParameterExpression. Got: {memberAccessor.Expression}");
        }

        if (item.Sort == MuiGridSortDirection.Desc)
        {
            // Extension methods cannot be dynamically dispatched
            return Queryable.OrderByDescending(query, expression);
        }

        // Extension methods cannot be dynamically dispatched
        return Queryable.OrderBy(query, expression);
    }
}