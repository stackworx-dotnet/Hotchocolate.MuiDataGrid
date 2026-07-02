namespace Stackworx.Hotchocolate.MuiDataGrid.Types;

using System.Text.Json;
using HotChocolate.Features;
using HotChocolate.Language;
using HotChocolate.Text.Json;
using HotChocolate.Types;

/// <summary>
/// A flexible ("Any"-like) scalar for MUI DataGrid filter values, which may be a string, number,
/// boolean, or a list thereof. Ported to the HotChocolate 16 <see cref="ScalarType{T}"/> coercion
/// model (CoerceInputLiteral / CoerceInputValue / OnCoerceOutputValue / OnValueToLiteral).
/// </summary>
public sealed class MuiValueType : ScalarType<MuiValue>
{
    public MuiValueType()
        : this("MuiValue")
    {
    }

    public MuiValueType(
        string name,
        string? description = null,
        BindingBehavior bind = BindingBehavior.Explicit)
        : base(name, bind)
    {
        this.Description = description;
    }

    /// <inheritdoc />
    public override ScalarSerializationType SerializationType => ScalarSerializationType.Any;

    /// <inheritdoc />
    public override bool IsValueCompatible(IValueNode valueLiteral)
        => valueLiteral.Kind is
            SyntaxKind.StringValue or
            SyntaxKind.IntValue or
            SyntaxKind.FloatValue or
            SyntaxKind.BooleanValue or
            SyntaxKind.ListValue or
            SyntaxKind.ObjectValue or
            SyntaxKind.NullValue;

    /// <inheritdoc />
    public override bool IsValueCompatible(JsonElement inputValue)
        => inputValue.ValueKind is
            JsonValueKind.String or
            JsonValueKind.Number or
            JsonValueKind.True or
            JsonValueKind.False or
            JsonValueKind.Array or
            JsonValueKind.Object or
            JsonValueKind.Null;

    /// <inheritdoc />
    public override object CoerceInputLiteral(IValueNode valueLiteral)
    {
        switch (valueLiteral)
        {
            case StringValueNode svn:
                return new MuiValue(svn.Value);
            case IntValueNode ivn:
                return new MuiValue(ivn.Value);
            case FloatValueNode fvn:
                return new MuiValue(fvn.Value);
            case BooleanValueNode bvn:
                return new MuiValue(bvn.Value ? "true" : "false");
            case ObjectValueNode ovn:
                var valueField = ovn.Fields.SingleOrDefault(f => f.Name.Value == "value")
                    ?? throw new ArgumentException("Expected field with name 'value'");
                if (valueField.Value is StringValueNode n)
                {
                    return new MuiValue(n.Value);
                }

                throw new ArgumentException($"Expected StringValueNode, got {valueField.Value.Kind}");
            case ListValueNode lvn:
                var items = new List<string>();
                foreach (var node in lvn.Items)
                {
                    items.Add(node switch
                    {
                        StringValueNode s => s.Value,
                        IntValueNode i => i.Value,
                        FloatValueNode f => f.Value,
                        BooleanValueNode b => b.Value ? "true" : "false",
                        _ => throw new ArgumentException($"Expected a scalar node. Got: {node.GetType()}"),
                    });
                }

                return new MuiValue(items);
            default:
                throw new ArgumentException(
                    $"{this.Name} cannot parse the given literal of type {valueLiteral.GetType()}");
        }
    }

    /// <inheritdoc />
    public override object CoerceInputValue(JsonElement inputValue, IFeatureProvider context)
    {
        switch (inputValue.ValueKind)
        {
            case JsonValueKind.String:
                return new MuiValue(inputValue.GetString()!);
            case JsonValueKind.Number:
                return new MuiValue(inputValue.GetRawText());
            case JsonValueKind.True:
                return new MuiValue("true");
            case JsonValueKind.False:
                return new MuiValue("false");
            case JsonValueKind.Array:
                var items = new List<string>();
                foreach (var element in inputValue.EnumerateArray())
                {
                    items.Add(element.ValueKind == JsonValueKind.String
                        ? element.GetString()!
                        : element.GetRawText());
                }

                return new MuiValue(items);
            case JsonValueKind.Object:
                if (inputValue.TryGetProperty("value", out var valueProp)
                    && valueProp.ValueKind == JsonValueKind.String)
                {
                    return new MuiValue(valueProp.GetString()!);
                }

                throw new ArgumentException("Expected an object with a string 'value' field.");
            default:
                throw new ArgumentException(
                    $"{this.Name} cannot parse the given JSON value of kind {inputValue.ValueKind}");
        }
    }

    /// <inheritdoc />
    protected override void OnCoerceOutputValue(MuiValue runtimeValue, ResultElement resultValue)
    {
        // MuiValue is an input scalar (filter values); output coercion emits its string form.
        if (runtimeValue.Value is null)
        {
            resultValue.SetNullValue();
            return;
        }

        resultValue.SetStringValue(runtimeValue.AsString());
    }

    /// <inheritdoc />
    protected override IValueNode OnValueToLiteral(MuiValue runtimeValue)
    {
        if (runtimeValue.Value is null)
        {
            return NullValueNode.Default;
        }

        if (runtimeValue.Value is string s)
        {
            return new StringValueNode(s);
        }

        if (runtimeValue.Value is IEnumerable<MuiValue> list)
        {
            return new ListValueNode(list.Select(this.OnValueToLiteral).ToList());
        }

        return new StringValueNode(runtimeValue.AsString());
    }
}
