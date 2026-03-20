namespace Stackworx.Hotchocolate.MuiDataGrid.Types;

using System.Collections;
using HotChocolate.Language;
using HotChocolate.Types;

public sealed class MuiValueType : ScalarType
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

    public override Type RuntimeType => typeof(MuiValue);

    public override bool IsInstanceOfType(IValueNode literal)
    {
        if (literal is null)
        {
            throw new ArgumentNullException(nameof(literal));
        }

        switch (literal)
        {
            case StringValueNode:
            case IntValueNode:
            case FloatValueNode:
            case BooleanValueNode:
            case ListValueNode:
            case ObjectValueNode:
            case NullValueNode:
                return true;

            default:
                return false;
        }
    }

    public override object? ParseLiteral(IValueNode literal)
    {
        switch (literal)
        {
            case StringValueNode svn:
                return new MuiValue(svn.Value);
            case IntValueNode ivn:
                return new MuiValue(ivn.Value);
            case FloatValueNode fvn:
                return new MuiValue(fvn.Value);
            case ObjectValueNode ovn:
                var valueField = ovn.Fields.SingleOrDefault(f => f.Name.Value == "value")
                                 ?? throw new ArgumentException("Expected field with name 'value'");

                if (valueField.Value is StringValueNode n)
                {
                    return new MuiValue(n.Value);
                }

                throw new ArgumentException($"Expected StringValueNode, got {valueField.Value.Kind}");
            case ListValueNode lvn:
                {
                    var items = new List<string>();
                    foreach (var node in lvn.Items)
                    {
                        if (node is StringValueNode svn)
                        {
                            items.Add(svn.Value);
                        }
                        else
                        {
                            // TODO: If object, for option case we need to get the value from the object
                            throw new ArgumentException($"Expected a string node. Got: {node.GetType()}");
                        }
                    }

                    return new MuiValue(items);
                }

            case NullValueNode:
                return null;
            default:
                throw new ArgumentException($"{this.Name} cannot parse the given literal of type {literal.GetType()}");
        }
    }

    public override IValueNode ParseValue(object? value)
    {
        return value is null
            ? NullValueNode.Default
            : this.ParseValue(value, new HashSet<object>());
    }

    public override IValueNode ParseResult(object? resultValue) =>
        this.ParseValue(resultValue);

    public override bool TrySerialize(object? runtimeValue, out object? resultValue)
    {
        throw new NotImplementedException();
    }

    public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
    {
        object? elementValue;
        runtimeValue = null;
        switch (resultValue)
        {
            case IDictionary<string, object> dictionary:
                {
                    var result = new Dictionary<string, object?>();
                    foreach (KeyValuePair<string, object> element in dictionary)
                    {
                        if (this.TryDeserialize(element.Value, out elementValue))
                        {
                            result[element.Key] = elementValue;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    runtimeValue = result;
                    return true;
                }

            case IList list:
                {
                    var result = new object?[list.Count];
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (this.TryDeserialize(list[i], out elementValue))
                        {
                            result[i] = elementValue;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    runtimeValue = result;
                    return true;
                }

            // TODO: this is only done for a bug in schema stitching and needs to be removed
            // once we have release stitching 2.
            case IValueNode literal:
                runtimeValue = this.ParseLiteral(literal);
                return true;

            default:
                runtimeValue = resultValue;
                return true;
        }
    }

    private IValueNode ParseValue(object? value, ISet<object> set)
    {
        if (value is null)
        {
            return NullValueNode.Default;
        }

        if (value is MuiValue muiValue)
        {
            switch (muiValue.Value)
            {
                case string s:
                    return new StringValueNode(s);
                case short s:
                    return new IntValueNode(s);
                case int i:
                    return new IntValueNode(i);
                case long l:
                    return new IntValueNode(l);
                case float f:
                    return new FloatValueNode(f);
                case double d:
                    return new FloatValueNode(d);
                case decimal d:
                    return new FloatValueNode(d);
                case bool b:
                    return new BooleanValueNode(b);
                    // TODO: list
                    // case sbyte s:
                    //     return new IntValueNode(s);
                    // case byte b:
                    //     return new IntValueNode(b);
            }
        }
        else
        {
            throw new ArgumentException($"Expected MuiValue. got: {value.GetType()}");
        }

        if (set.Add(value))
        {
            if (value is IReadOnlyList<object> list)
            {
                var valueList = new List<IValueNode>();
                foreach (object element in list)
                {
                    valueList.Add(this.ParseValue(element, set));
                }

                return new ListValueNode(valueList);
            }

            // return ParseValue(_objectToDictConverter.Convert(value), set);
        }

        throw new ArgumentException("cycle in object graphql");
    }
}
