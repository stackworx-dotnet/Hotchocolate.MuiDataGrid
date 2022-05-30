namespace Stackworx.Hotchocolate.MuiDataGrid;

public class DefaultEnumSingleSelectHandler<T, TEnum> : DefaultSingleSelectHandler<T>
    where TEnum : struct, Enum
{
    public override ConstantExpression GetValueConstantExpression(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        var v = filter.Value.AsString();
        if (Enum.TryParse<TEnum>(v, out var g))
        {
            return Expression.Constant(g);
        }

        throw new ArgumentException($"Failed to Parse {nameof(TEnum)}: {v}");
    }

    public override ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsArray().Select(e =>
        {
            var v = e.AsString();
            if (Enum.TryParse<TEnum>(v, out var g))
            {
                return g;
            }

            throw new ArgumentException($"Failed to Parse {nameof(TEnum)}: {v}");
        }).ToList());
    }
}