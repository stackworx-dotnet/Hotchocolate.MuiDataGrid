namespace Stackworx.Hotchocolate.MuiDataGrid.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

public class MuiGridSortDirectionConverter : JsonConverter<MuiGridSortDirection>
{
    public override MuiGridSortDirection Read(
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
            "asc" => MuiGridSortDirection.Asc,
            "Asc" => MuiGridSortDirection.Asc,
            "desc" => MuiGridSortDirection.Desc,
            "Desc" => MuiGridSortDirection.Desc,
            _ => throw new JsonException($"Could not parse {nameof(MuiGridSortDirection)}: {s}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, MuiGridSortDirection value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}