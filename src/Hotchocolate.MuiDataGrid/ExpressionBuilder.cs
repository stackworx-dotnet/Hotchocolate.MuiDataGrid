namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Language;

public class ExpressionBuilder<T>(IColumnLookup<T> columnLookup, ExpressionBuilderFlavour flavour = ExpressionBuilderFlavour.EFCORE)
{
    private readonly Dictionary<string, IExpressionBuilderHandler<T>> handlers = new();

    private readonly IExpressionBuilderHandler<T> defaultBooleanHandler = new DefaultBooleanHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultStringHandler = new DefaultStringHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultNumberHandler = new DefaultNumberHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultSingleSelectHandler = new DefaultSingleSelectHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultDateTimeHandler = new DefaultDateTimeHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultDateOnlyHandler = new DefaultDateOnlyHandler<T>();
    private readonly IExpressionBuilderHandler<T> defaultGuidHandler = new DefaultGuidHandler<T>();

    public void AddHandler(string columnField, IExpressionBuilderHandler<T> handler)
    {
        // Validate column
        if (!columnLookup.CanHandle(columnField))
        {
            throw new ArgumentException($"{columnLookup} cannot handle column {columnField}");
        }

        if (!this.handlers.TryAdd(columnField, handler))
        {
            throw new ArgumentException($"Handler for column: {columnField} already registered");
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
        var predicates = filters.Items.Select(this.Build).ToList();
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

    public Expression<Func<T, bool>> FilterWithConstraint(
        MuiDataGridFilterInput filters,
        Expression<Func<T, bool>> additionalConstraint)
    {
        var builderFilter = this.Filter(filters);

        // Remove Convert operations from the constraint
        var normalizedConstraint = RemoveConverts(additionalConstraint);

        return CombineAnd(builderFilter, normalizedConstraint);
    }

    private static Expression<Func<T, bool>> RemoveConverts(Expression<Func<T, bool>> expression)
    {
        var remover = new ConvertRemover();
        var body = remover.Visit(expression.Body);
        var result = Expression.Lambda<Func<T, bool>>(body, expression.Parameters);
        return result;
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

    private static Expression<Func<T, bool>> CombineAnd(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        // Normalize both expressions to remove Convert operations
        var normalizedExpr1 = new ConvertRemover().Visit(expr1) as Expression<Func<T, bool>> ?? expr1;
        var normalizedExpr2 = new ConvertRemover().Visit(expr2) as Expression<Func<T, bool>> ?? expr2;

        var parameter = normalizedExpr1.Parameters[0];
        var visitor = new ReplaceParameterVisitor(normalizedExpr2.Parameters[0], parameter);
        var body2 = visitor.Visit(normalizedExpr2.Body);

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(normalizedExpr1.Body, body2), parameter);
    }

    // MemberExpression memberAccessor,
    private Expression<Func<T, bool>> Build(MuiDataGridFilterItemInput filter)
    {
        var memberAccessor = columnLookup.Lookup(filter.Field);

        if (this.handlers.TryGetValue(filter.Field, out var handler))
        {
            return handler.Handle(memberAccessor, flavour, filter);
        }

        var t = memberAccessor.Type.UnwrapNullable();

        switch (t)
        {
            case var _ when t == typeof(int):
            case var _ when t == typeof(double):
            case var _ when t == typeof(float):
            case var _ when t == typeof(short):
            case var _ when t == typeof(decimal):
                return this.defaultNumberHandler.Handle(memberAccessor, flavour, filter);
            case var _ when t == typeof(string):
                return this.defaultStringHandler.Handle(memberAccessor, flavour, filter);
            case var _ when t == typeof(bool):
                return this.defaultBooleanHandler.Handle(memberAccessor, flavour, filter);
            case var _ when t == typeof(DateTime):
            case var _ when t == typeof(DateTimeOffset):
                return this.defaultDateTimeHandler.Handle(memberAccessor, flavour, filter);
            case var _ when t == typeof(DateOnly):
                return this.defaultDateOnlyHandler.Handle(memberAccessor, flavour, filter);
            case var _ when t == typeof(Guid):
                return this.defaultGuidHandler.Handle(memberAccessor, flavour, filter);
            default:
                throw new ArgumentException($"Unexpected Member Type {t}");
        }
    }

    private IQueryable<T> Sort(IQueryable<T> query, MuiDataGridSortItem item)
    {
        var member = columnLookup.Lookup(item.Field);
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

        return item.Sort == MuiGridSortDirection.Desc
            ? (IQueryable<T>)Queryable.OrderByDescending(query, expression)
            : (IQueryable<T>)Queryable.OrderBy(query, expression);
    }

    private class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParameter ? newParameter : base.VisitParameter(node);
        }
    }

    private class ConvertRemover : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var left = this.Visit(node.Left);
            var right = this.Visit(node.Right);

            // If we removed a Convert and types don't match, fix the constant
            if (node.NodeType is ExpressionType.Equal or ExpressionType.NotEqual && left.Type != right.Type)
            {
                // If right side is a constant integer and left is an enum
                if (right is ConstantExpression constExpr && constExpr.Type == typeof(int) && left.Type.IsEnum)
                {
                    if (constExpr.Value is not null)
                    {
                        // Convert the int constant to the enum type
                        right = Expression.Constant(Enum.ToObject(left.Type, constExpr.Value), left.Type);
                    }
                }
                else if (left is ConstantExpression leftConst && leftConst.Type == typeof(int) && right.Type.IsEnum)
                {
                    // If left side is a constant integer and right is an enum
                    if (leftConst.Value is not null)
                    {
                        left = Expression.Constant(Enum.ToObject(right.Type, leftConst.Value), right.Type);
                    }
                }
            }

            if (left != node.Left || right != node.Right)
            {
                return Expression.MakeBinary(node.NodeType, left, right);
            }

            return base.VisitBinary(node);
        }
    }
}
