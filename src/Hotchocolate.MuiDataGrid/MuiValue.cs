namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json.Nodes;

public record MuiValue
{
    private readonly JsonNode node;

    public MuiValue(string val)
    {
        this.node = JsonNode.Parse(val) ?? throw new InvalidOperationException();
    }

    internal MuiValue(JsonNode node)
    {
        this.node = node;
    }

    public object AsNumber(Type memberType)
    {
        switch (memberType)
        {
            case var x when x == typeof(int):
                return this.AsInt();
            case var x when x == typeof(double):
                return this.AsDouble();
            case var x when x == typeof(float):
                return this.AsFloat();
            case var x when x == typeof(short):
                return this.AsShort();
            default:
                throw new ArgumentException($"Invalid type: {memberType}");
        }
    }

    public string AsString()
    {
        return this.node.GetValue<string>();
    }

    public DateOnly AsDateOnly()
    {
        return this.node.GetValue<DateOnly>();
    }

    public DateTime AsDateTime()
    {
        return this.node.GetValue<DateTime>();
    }

    public int AsInt()
    {
        return int.Parse(this.node.GetValue<string>());
    }

    public double AsDouble()
    {
        return double.Parse(this.node.GetValue<string>());
    }

    public float AsFloat()
    {
        return float.Parse(this.node.GetValue<string>());
    }

    public short AsShort()
    {
        return short.Parse(this.node.GetValue<string>());
    }

    public bool AsBoolean()
    {
        return bool.Parse(this.node.GetValue<string>());
    }

    public string ToJsonString()
    {
        return this.node.ToJsonString();
    }

    public IEnumerable<MuiValue> AsArray()
    {
        var arr = this.node.AsArray();
        // TODO: error if any nulls
        return arr.Where(x => x != null)
            .Cast<JsonNode>()
            .Select(a => new MuiValue(a));
    }
}