namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ClientQueryRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;

    [JsonPropertyName("variables")]
    public Dictionary<string, object> Variables { get; set; } = new();

    [JsonPropertyName("extensions")]
    public Dictionary<string, object> Extensions { get; set; } = new();

    public override string ToString()
    {
        var query = new StringBuilder();

        query.Append($"id={this.Id}");
        query.Append('&');
        query.Append($"query={this.Query.Replace("\r", string.Empty).Replace("\n", string.Empty)}");
        // query.Append($"&operationName={this.OperationName}");
        query.Append("&variables=" + JsonSerializer.Serialize(this.Variables));
        query.Append("&extensions=" + JsonSerializer.Serialize(this.Extensions));

        return query.ToString();
    }
}