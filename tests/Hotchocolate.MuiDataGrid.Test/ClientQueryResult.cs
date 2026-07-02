namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Net;

public class ClientQueryResult
{
    public string ContentType { get; set; } = string.Empty;

    public HttpStatusCode StatusCode { get; set; }

    public Dictionary<string, object> Data { get; set; } = new();

    public IReadOnlyList<ClientQueryError> Errors { get; set; } = [];

    public Dictionary<string, object> Extensions { get; set; } = new();

    // Incremental delivery (defer/stream) payloads — unused by the filter/sort tests.
    public IReadOnlyList<object>? Incremental { get; set; }

    public Dictionary<string, object> ContextData { get; set; } = new();

    public bool? HasNext { get; set; }

    public bool IsDataSet { get; set; }
}