namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Language;
using HotChocolate.Types;

public sealed class MuiValueScalar : ScalarType<MuiValue, StringValueNode>
{
    public MuiValueScalar()
        : base("MuiValue")
    {
    }

    public override IValueNode ParseResult(object? resultValue)
    {
        return this.ParseValue(resultValue);
    }

    protected override MuiValue ParseLiteral(StringValueNode valueSyntax)
    {
        // TODO: do early check for valid types.
        // TODO: null check
        return new MuiValue(valueSyntax.Value);
    }

    protected override StringValueNode ParseValue(MuiValue runtimeValue)
    {
        // Note: the tests uses this because we serialize to json
        return new StringValueNode(runtimeValue.ToJsonString());
    }
}