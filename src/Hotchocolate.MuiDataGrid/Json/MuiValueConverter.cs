namespace Stackworx.Hotchocolate.MuiDataGrid.Json;

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public class MuiValueConverter : JsonConverter<MuiValue>
{
    public override MuiValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);
        if (node != null)
        {
            return new MuiValue(node);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, MuiValue value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}