namespace Stackworx.Hotchocolate.MuiDataGrid;

public sealed class DataTypePropertyBuilder<T, TProperty>(DataTypeBuilder<T> builder, string fieldName)
{
    private string currentFieldName = fieldName;

    public DataTypePropertyBuilder<T, TProperty> SetName(string fieldNameOverride)
    {
        builder.SetFieldName(this.currentFieldName, fieldNameOverride);
        this.currentFieldName = fieldNameOverride;
        return this;
    }

    public DataTypePropertyBuilder<T, TProperty> SetHandler(IExpressionBuilderHandler<T> handler)
    {
        builder.SetHandler(this.currentFieldName, handler);
        return this;
    }
}
