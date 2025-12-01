namespace Stackworx.Hotchocolate.MuiDataGrid;

public class ValueRequiredError(string operationName) : Exception($"Value is required for operation {operationName}");