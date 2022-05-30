namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Diagnostics.CodeAnalysis;

public static class MuiValueExtensions
{
    public static void AssertNotNull([NotNull] this MuiValue? value, string operationName)
    {
        if (value == null)
        {
            throw new ValueRequiredError(operationName);
        }
    }
}