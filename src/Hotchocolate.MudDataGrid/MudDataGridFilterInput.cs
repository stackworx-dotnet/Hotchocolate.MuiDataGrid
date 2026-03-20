namespace Stackworx.Hotchocolate.MudDataGrid;

public record MudDataGridFilterInput
{
    // Mirrors MudBlazor GridState collections for server-side handling.
    public IList<MudDataGridFilterDefinitionInput> FilterDefinitions { get; init; } = [];

    // Kept as a list to preserve insertion order over GraphQL transport.
    public IList<MudDataGridSortDefinitionInput> SortDefinitions { get; init; } = [];
}