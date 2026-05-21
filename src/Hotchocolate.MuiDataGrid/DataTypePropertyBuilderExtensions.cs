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

    /// <summary>
    /// Registers <see cref="DefaultEnumMultiSelectHandler{T,TEnum}"/> for an
    /// <see cref="IEnumerable{TEnum}"/> property. Only the <c>isAnyOf</c> operator
    /// is supported: rows are returned when the collection contains any of the
    /// selected enum values.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TEnum">The enum type stored in the collection.</typeparam>
    /// <param name="builder">The property builder to configure.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static DataTypePropertyBuilder<T, IEnumerable<TEnum>> SetEnumMultiSelectHandler<T, TEnum>(
        this DataTypePropertyBuilder<T, IEnumerable<TEnum>> builder)
        where TEnum : struct, Enum
    {
        return builder.SetHandler(new DefaultEnumMultiSelectHandler<T, TEnum>());
    }

    /// <summary>
    /// Registers <see cref="DefaultEnumMultiSelectHandler{T,TEnum}"/> for an
    /// <see cref="ICollection{TEnum}"/> property. Only the <c>isAnyOf</c> operator
    /// is supported.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TEnum">The enum type stored in the collection.</typeparam>
    /// <param name="builder">The property builder to configure.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static DataTypePropertyBuilder<T, ICollection<TEnum>> SetEnumMultiSelectHandler<T, TEnum>(
        this DataTypePropertyBuilder<T, ICollection<TEnum>> builder)
        where TEnum : struct, Enum
    {
        return builder.SetHandler(new DefaultEnumMultiSelectHandler<T, TEnum>());
    }

    /// <summary>
    /// Registers <see cref="DefaultEnumMultiSelectHandler{T,TEnum}"/> for an
    /// <see cref="IList{TEnum}"/> property. Only the <c>isAnyOf</c> operator is
    /// supported.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TEnum">The enum type stored in the collection.</typeparam>
    /// <param name="builder">The property builder to configure.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static DataTypePropertyBuilder<T, IList<TEnum>> SetEnumMultiSelectHandler<T, TEnum>(
        this DataTypePropertyBuilder<T, IList<TEnum>> builder)
        where TEnum : struct, Enum
    {
        return builder.SetHandler(new DefaultEnumMultiSelectHandler<T, TEnum>());
    }

    /// <summary>
    /// Registers <see cref="DefaultEnumMultiSelectHandler{T,TEnum}"/> for a
    /// <see cref="List{TEnum}"/> property. Only the <c>isAnyOf</c> operator is
    /// supported.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TEnum">The enum type stored in the collection.</typeparam>
    /// <param name="builder">The property builder to configure.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static DataTypePropertyBuilder<T, List<TEnum>> SetEnumMultiSelectHandler<T, TEnum>(
        this DataTypePropertyBuilder<T, List<TEnum>> builder)
        where TEnum : struct, Enum
    {
        return builder.SetHandler(new DefaultEnumMultiSelectHandler<T, TEnum>());
    }

    public static DataTypePropertyBuilder<T, TProperty> SetNodeIdHandler<T, TProperty>(
        this DataTypePropertyBuilder<T, TProperty> builder,
        INodeIdSerializer idSerializer,
        string? relayType = null)
    {
        return builder.SetHandler(new DefaultRelayIdSingleSelectHandler<T>(idSerializer, relayType));
    }
}
