namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text;
using Newtonsoft.Json;

public class ClientQueryRequest
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("operationName")]
    public string OperationName { get; set; } = string.Empty;

    [JsonProperty("query")]
    public string Query { get; set; } = string.Empty;

    [JsonProperty("variables")]
    public Dictionary<string, object> Variables { get; set; } = new();

    [JsonProperty("extensions")]
    public Dictionary<string, object> Extensions { get; set; } = new();

    public override string ToString()
    {
        var query = new StringBuilder();

        if (this.Id is not null)
        {
            query.Append($"id={this.Id}");
        }

        if (this.Query is not null)
        {
            if (this.Id is not null)
            {
                query.Append("&");
            }

            query.Append($"query={this.Query.Replace("\r", string.Empty).Replace("\n", string.Empty)}");
        }

        if (this.OperationName is not null)
        {
            query.Append($"&operationName={this.OperationName}");
        }

        if (this.Variables is not null)
        {
            query.Append("&variables=" + JsonConvert.SerializeObject(this.Variables));
        }

        if (this.Extensions is not null)
        {
            query.Append("&extensions=" + JsonConvert.SerializeObject(this.Extensions));
        }

        return query.ToString();
    }
}
