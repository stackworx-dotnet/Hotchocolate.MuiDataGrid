namespace Stackworx.Hotchocolate.MuiDataGrid;

public class ClientQueryError
{
    public string Message { get; set; } = string.Empty;

    public IReadOnlyList<Location> Locations { get; set; } = [];

    public IReadOnlyList<string> Path { get; set; } = [];

    public Dictionary<string, string?> Extensions { get; set; } = new();

    public class Location
    {
        public int Line { get; set; }

        public int Column { get; set; }
    }
}
