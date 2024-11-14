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
        var id = idSerializer.Parse(value.AsString(), typeof(T));
        if (relayType != null && id.TypeName != relayType)
        {
            throw new ArgumentException($"Expected Type: {this.relayType} got: {id.TypeName}");
        }

        var type = member.Type.UnwrapNullable();

        return type switch
        {
            _ when type == typeof(string) => id.Value.ToString()!,
            _ when type == typeof(Guid) => Guid.Parse(id.Value.ToString()!),
            _ when type == typeof(int) => int.Parse(id.Value.ToString()!),
            _ when type == typeof(long) => long.Parse(id.Value.ToString()!),
            _ when type == typeof(short) => short.Parse(id.Value.ToString()!),
            _ => throw new ArgumentException($"Invalid type: {member.Type}"),
        };
    }
}
