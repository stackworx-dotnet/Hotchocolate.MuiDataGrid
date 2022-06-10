namespace Stackworx.Hotchocolate.MuiDataGrid;

public record MuiValue
{
    public MuiValue(IList<string> val)
    {
        this.Value = val.Select(s => new MuiValue(s));
    }

    public MuiValue(string val)
    {
        this.Value = val;
    }

    public dynamic? Value { get; }

    public dynamic AsNumber(Type memberType)
    {
        switch (memberType)
        {
            case var x when x == typeof(int):
                return int.Parse(this.AsString());
            case var x when x == typeof(double):
                return double.Parse(this.AsString());
            case var x when x == typeof(float):
                return float.Parse(this.AsString());
            case var x when x == typeof(short):
                return short.Parse(this.AsString());
            default:
                throw new ArgumentException($"Invalid type: {memberType}");
        }
    }

    public string AsString()
    {
        if (this.Value is null)
        {
            throw new ArgumentException("value is null");
        }

        return this.Value.ToString();
    }

    public IEnumerable<MuiValue> AsArray()
    {
        if (this.Value is null)
        {
            throw new ArgumentException("value is null");
        }

        return this.Value;
    }

    public override string ToString()
    {
        return this.Value?.ToString() ?? "[null]";
    }
}