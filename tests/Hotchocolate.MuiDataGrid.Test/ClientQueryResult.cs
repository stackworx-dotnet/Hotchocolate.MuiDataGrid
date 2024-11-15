namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Net;
using HotChocolate.Execution;

public class ClientQueryResult
{
    public string ContentType { get; set; } = string.Empty;

    public HttpStatusCode StatusCode { get; set; }

    public Dictionary<string, object> Data { get; set; } = new();

    public IReadOnlyList<IError> Errors { get; set; } = [];

    public Dictionary<string, object> Extensions { get; set; } = new();

    public IReadOnlyList<IOperationResult>? Incremental { get; set; }

    public Dictionary<string, object> ContextData { get; set; } = new();

    public bool? HasNext { get; set; }

    public bool IsDataSet { get; set; }
}