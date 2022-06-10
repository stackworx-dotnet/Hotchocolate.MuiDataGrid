namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Net;

public class ClientQueryResult
{
    public string ContentType { get; set; } = string.Empty;

    public HttpStatusCode StatusCode { get; set; }

    public Dictionary<string, object> Data { get; set; } = new();

    public List<Dictionary<string, object>> Errors { get; set; } = new();

    public Dictionary<string, object> Extensions { get; set; } = new();
}
