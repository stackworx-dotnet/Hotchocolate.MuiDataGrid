namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

using System.Linq.Expressions;
using Stackworx.Hotchocolate.MuiDataGrid;

public class PersonGenderSingleSelectHandler : DefaultSingleSelectHandler<Person>
{
    public override ConstantExpression GetValueConstantExpression(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        var v = filter.Value.AsString();
        if (Enum.TryParse<Gender>(v, out var g))
        {
            return Expression.Constant(g);
        }

        throw new ArgumentException($"Failed to Parse Gender: {v}");
    }

    public override ConstantExpression GetValueConstantExpressionList(ColumnLookupMember member, MuiDataGridFilterItemInput filter)
    {
        filter.Value.AssertNotNull(filter.OperatorValue);
        return Expression.Constant(filter.Value.AsArray().Select(e =>
        {
            var v = e.AsString();
            if (Enum.TryParse<Gender>(v, out var g))
            {
                return g;
            }

            throw new ArgumentException($"Failed to Parse Gender: {v}");
        }).ToList());
    }
}