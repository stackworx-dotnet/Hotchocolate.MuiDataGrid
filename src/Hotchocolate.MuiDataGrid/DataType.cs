namespace Stackworx.Hotchocolate.MuiDataGrid;

using JetBrains.Annotations;

[PublicAPI]
[method: UsedImplicitly]
public abstract class DataType<T>(ExpressionBuilderFlavour flavour)
{
    private readonly ExpressionBuilderFlavour flavour = flavour;
    private bool initialized;
    private IReadOnlyDictionary<string, ColumnLookupMember> members = new Dictionary<string, ColumnLookupMember>(StringComparer.Ordinal);
    private IReadOnlyDictionary<string, IExpressionBuilderHandler<T>> handlers =
        new Dictionary<string, IExpressionBuilderHandler<T>>(StringComparer.Ordinal);

    [UsedImplicitly]
    protected DataType()
        : this(ExpressionBuilderFlavour.EFCORE)
    {
    }

    public ExpressionBuilderFlavour Flavor { get; set; } = flavour;

    internal IReadOnlyDictionary<string, IExpressionBuilderHandler<T>> Handlers
    {
        get
        {
            this.EnsureInitialized();
            return this.handlers;
        }
    }

    public IQueryable<T> Sort(IQueryable<T> query, IList<MuiDataGridSortItem> items)
    {
        var builder = new ExpressionBuilder<T>(this, this.flavour);
        return builder.Sort(query, items);
    }

    public Expression<Func<T, bool>> Filter(MuiDataGridFilterInput filters)
    {
        var builder = new ExpressionBuilder<T>(this, this.flavour);
        return builder.Filter(filters);
    }

    public Expression<Func<T, bool>> FilterWithConstraint(
        MuiDataGridFilterInput filters,
        Expression<Func<T, bool>> additionalConstraint)
    {
        var builder = new ExpressionBuilder<T>(this, this.flavour);
        return builder.FilterWithConstraint(filters, additionalConstraint);
    }

    internal ColumnLookupMember Lookup(string column)
    {
        this.EnsureInitialized();

        if (this.members.TryGetValue(column, out var member))
        {
            return member;
        }

        throw new ArgumentException($"Unhandled Column: {column}");
    }

    protected abstract void Configure(DataTypeBuilder<T> builder);

    private void EnsureInitialized()
    {
        if (this.initialized)
        {
            return;
        }

        var builder = new DataTypeBuilder<T>();
        this.Configure(builder);

        this.members = builder.BuildMembers();
        this.handlers = builder.BuildHandlers();
        this.initialized = true;
    }
}
