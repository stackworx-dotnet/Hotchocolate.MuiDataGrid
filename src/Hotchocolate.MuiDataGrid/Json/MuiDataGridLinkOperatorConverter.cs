namespace Stackworx.Hotchocolate.MuiDataGrid.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

public class MuiDataGridLinkOperatorConverter : JsonConverter<MuiDataGridLogicOperator>
{
    public override MuiDataGridLogicOperator Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string, Got: {reader.TokenType}");
        }

        var s = reader.GetString();
        ArgumentNullException.ThrowIfNull(s);
        return s.ToLower() switch
        {
            "and" => MuiDataGridLogicOperator.And,
            "or" => MuiDataGridLogicOperator.Or,
            _ => throw new JsonException($"Could not parse {nameof(MuiDataGridLogicOperator)}: {s}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, MuiDataGridLogicOperator value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}