namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Types.Relay;

public class DefaultRelayIdSingleSelectHandler<T>(INodeIdSerializer idSerializer, string? relayType = null) : DefaultSingleSelectHandler<T>
{
    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        var id = idSerializer.Parse(value.AsString(), member.Type);
        if (relayType != null && id.TypeName != relayType)
        {
            throw new ArgumentException($"Expected Type: {relayType} got: {id.TypeName}");
        }

        var type = member.Type.UnwrapNullable();

        return type switch
        {
            _ when type == typeof(string) => id.InternalId.ToString()!,
            _ when type == typeof(Guid) => Guid.Parse(id.InternalId.ToString()!),
            _ when type == typeof(int) => int.Parse(id.InternalId.ToString()!),
            _ when type == typeof(long) => long.Parse(id.InternalId.ToString()!),
            _ when type == typeof(short) => short.Parse(id.InternalId.ToString()!),
            _ => throw new ArgumentException($"Invalid type: {member.Type}"),
        };
    }
}
