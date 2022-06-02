namespace Stackworx.Hotchocolate.MuiDataGrid.EnumTypes;

using HotChocolate.Types;

public class LinkOperatorEnumType : EnumType<MuiDataGridLinkOperator>
{
    protected override void Configure(IEnumTypeDescriptor<MuiDataGridLinkOperator> descriptor)
    {
        descriptor.Name("LinkOperator");

        descriptor.Value(MuiDataGridLinkOperator.Or).Name("or");
        descriptor.Value(MuiDataGridLinkOperator.And).Name("and");
    }
}