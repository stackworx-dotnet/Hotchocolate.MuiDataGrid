namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Types.Relay;

public static class DataTypePropertyBuilderExtensions
{
    public static DataTypePropertyBuilder<T, TEnum> SetEnumHandler<T, TEnum>(
        this DataTypePropertyBuilder<T, TEnum> builder)
        where TEnum : struct, Enum
    {
        return builder.SetHandler(new DefaultEnumSingleSelectHandler<T, TEnum>());
    }

    public static DataTypePropertyBuilder<T, TProperty> SetNodeIdHandler<T, TProperty>(
        this DataTypePropertyBuilder<T, TProperty> builder,
        INodeIdSerializer idSerializer,
        string? relayType = null)
    {
        return builder.SetHandler(new DefaultRelayIdSingleSelectHandler<T>(idSerializer, relayType));
    }
}
