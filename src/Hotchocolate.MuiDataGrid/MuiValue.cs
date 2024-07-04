namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Globalization;

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
        var numberFormatProvider = new NumberFormatInfo
        {
            NumberDecimalSeparator = ".",
            NumberDecimalDigits = 6,
        };
        switch (memberType)
        {
            case var x when x == typeof(int) || x == typeof(int?):
                return int.TryParse(
                    this.AsString(),
                    out var i)
                    ? i
                    : 0;
            case var x when x == typeof(double) || x == typeof(double?):
                return double.TryParse(
                    this.AsString(),
                    NumberStyles.AllowDecimalPoint,
                    numberFormatProvider,
                    out var d)
                    ? d
                    : 0d;
            case var x when x == typeof(float) || x == typeof(float?):
                return float.TryParse(
                    this.AsString(),
                    NumberStyles.AllowDecimalPoint,
                    numberFormatProvider,
                    out var f)
                    ? f
                    : 0f;
            case var x when x == typeof(short) || x == typeof(short?):
                return short.TryParse(
                    this.AsString(),
                    out var s)
                    ? s
                    : short.Parse("0");
            case var x when x == typeof(decimal) || x == typeof(decimal?):
                return decimal.TryParse(
                    this.AsString(),
                    NumberStyles.AllowDecimalPoint,
                    numberFormatProvider,
                    out var m)
                    ? m
                    : 0m;

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