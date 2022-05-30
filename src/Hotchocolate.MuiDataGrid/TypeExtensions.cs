namespace Stackworx.Hotchocolate.MuiDataGrid;

internal static class TypeExtensions
{
    public static Type UnwrapNullable(this Type value)
    {
        var inner = Nullable.GetUnderlyingType(value);

        if (inner != null)
        {
            return inner;
        }

        return value;
    }

    public static bool IsNullable(this Type value)
    {
        return Nullable.GetUnderlyingType(value) != null;
    }
}