namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Types.Relay;

public class DefaultRelayIdSingleSelectHandler<T> : DefaultSingleSelectHandler<T>
{
    private readonly IIdSerializer idSerializer;
    private readonly string? relayType;

    public DefaultRelayIdSingleSelectHandler(IIdSerializer idSerializer, string? relayType = null)
    {
        this.idSerializer = idSerializer;
        this.relayType = relayType;
    }

    public override ConstantExpression GetValueConstantExpression(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        var id = this.Deserialize(filter.Value);

        // TODO: what type should this be?
        return Expression.Constant(id);
    }

    public override ConstantExpression GetValueConstantExpressionList(
        ColumnLookupMember member,
        MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);

        var list = CreateGenericList(member.Type);

        foreach (var value in filter.Value.AsArray())
        {
            list.Add(this.Deserialize(value));
        }

        return Expression.Constant(list);
    }

    private static dynamic CreateGenericList(Type t)
    {
        var generic = typeof(List<>);
        var constructed = generic.MakeGenericType(t);
        return Activator.CreateInstance(constructed) ?? throw new InvalidOperationException();
    }

    private dynamic Deserialize(MuiValue value)
    {
        var id = this.idSerializer.Deserialize(value.AsString());
        if (this.relayType != null && id.TypeName != this.relayType)
        {
            throw new ArgumentException($"Expected Type: {this.relayType} got: {id.TypeName}");
        }

        return id.Value;
    }
}