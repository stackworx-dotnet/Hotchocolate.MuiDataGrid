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

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        var id = this.idSerializer.Deserialize(value.AsString());
        if (this.relayType != null && id.TypeName != this.relayType)
        {
            throw new ArgumentException($"Expected Type: {this.relayType} got: {id.TypeName}");
        }

        return id.Value;
    }
}