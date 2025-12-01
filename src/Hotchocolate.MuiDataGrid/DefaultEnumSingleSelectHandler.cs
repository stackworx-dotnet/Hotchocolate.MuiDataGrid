namespace Stackworx.Hotchocolate.MuiDataGrid;

using Humanizer;

public class DefaultEnumSingleSelectHandler<T, TEnum> : DefaultSingleSelectHandler<T>
    where TEnum : struct, Enum
{
    protected override dynamic ParseValue(ColumnLookupMember member, MuiValue value)
    {
        var v = value.AsString();
        v = v.Humanize(LetterCasing.Title).Transform(To.LowerCase, To.TitleCase).Dehumanize();
        if (Enum.TryParse<TEnum>(v, out var g))
        {
            return g;
        }

        throw new ArgumentException($"Failed to Parse {nameof(TEnum)}: {v}");
    }
}
