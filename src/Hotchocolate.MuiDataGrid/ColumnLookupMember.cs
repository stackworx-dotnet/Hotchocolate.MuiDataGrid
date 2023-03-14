namespace Stackworx.Hotchocolate.MuiDataGrid;

public record ColumnLookupMember(MemberExpression Expression, MemberInfo Member, Type Type,
    ParameterExpression ParameterExpression)
{
    public bool IsNullable
    {
        get
        {
            var nullabilityContext = new NullabilityInfoContext();
            if (this.Member is PropertyInfo propertyInfo)
            {
                var ctx = nullabilityContext.Create(propertyInfo);
                return ctx.ReadState == NullabilityState.Nullable;
            }

            return false;
        }
    }
}