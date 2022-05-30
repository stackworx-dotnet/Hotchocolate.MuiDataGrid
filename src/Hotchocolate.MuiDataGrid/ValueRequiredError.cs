namespace Stackworx.Hotchocolate.MuiDataGrid;

public class ValueRequiredError : Exception
{
    public ValueRequiredError(string operationName)
        : base($"Value is required for operation {operationName}")
    {
    }
}