namespace Stackworx.Hotchocolate.MuiDataGrid.EnumTypes;

using HotChocolate.Types;

public class MuiDataGridLinkOperatorEnumType : EnumType<MuiDataGridLinkOperator>
{
    protected override void Configure(IEnumTypeDescriptor<MuiDataGridLinkOperator> descriptor)
    {
        descriptor.Name("MuiDataGridLinkOperator");
        descriptor.Value(MuiDataGridLinkOperator.Or).Name("or");
        descriptor.Value(MuiDataGridLinkOperator.And).Name("and");
    }
}