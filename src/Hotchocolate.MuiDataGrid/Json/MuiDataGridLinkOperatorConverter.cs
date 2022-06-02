namespace Stackworx.Hotchocolate.MuiDataGrid.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

public class MuiDataGridLinkOperatorConverter : JsonConverter<MuiDataGridLinkOperator>
{
    public override MuiDataGridLinkOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string, Got: {reader.TokenType}");
        }

        var s = reader.GetString();
        return s switch
        {
            "and" => MuiDataGridLinkOperator.And,
            "or" => MuiDataGridLinkOperator.Or,
            _ => throw new JsonException($"Could not parse {nameof(MuiDataGridLinkOperator)}: {s}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, MuiDataGridLinkOperator value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}