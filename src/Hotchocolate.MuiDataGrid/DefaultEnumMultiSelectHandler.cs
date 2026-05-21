namespace Stackworx.Hotchocolate.MuiDataGrid;

using Humanizer;

/// <summary>
/// Handles filtering for entity properties that are collections of enum values.
/// Only the <c>isAnyOf</c> operator is supported: a row matches when the entity's
/// enum collection contains at least one of the selected filter values.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="TEnum">The enum type stored in the collection.</typeparam>
public class DefaultEnumMultiSelectHandler<T, TEnum> : ExpressionBuilderHandler<T>
    where TEnum : struct, Enum
{
    protected override Expression InternalHandle(ColumnLookupMember member, ExpressionBuilderFlavour flavour, MuiDataGridFilterItemInput filter)
    {
        return filter.Operator switch
        {
            "isAnyOf" => this.BuildIsAnyOf(member, filter),
            _ => throw new ArgumentException($"Unknown operator: {filter.Operator}"),
        };
    }

    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        var v = value.AsString();
        v = v.Humanize(LetterCasing.Title).Transform(To.LowerCase, To.TitleCase).Dehumanize();
        if (Enum.TryParse<TEnum>(v, out var result))
        {
            return result;
        }

        throw new ArgumentException($"Failed to Parse {typeof(TEnum).Name}: {v}");
    }

    // Builds: entity.Collection.Any(e => filterValues.Contains(e))
    private Expression BuildIsAnyOf(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.Operator);

        var enumValues = filter.Value
            .AsArray()
            .Select(v => (TEnum)this.ParseValue(member, v))
            .ToList();

        var filterConstant = Expression.Constant(enumValues, typeof(List<TEnum>));

        var enumParam = Expression.Parameter(typeof(TEnum), "e");
        var containsMethod = typeof(List<TEnum>).GetMethod("Contains", new[] { typeof(TEnum) })!;
        var containsCall = Expression.Call(filterConstant, containsMethod, enumParam);
        var predicate = Expression.Lambda<Func<TEnum, bool>>(containsCall, enumParam);

        var anyMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TEnum));

        return Expression.Call(anyMethod, member.Expression, predicate);
    }
}