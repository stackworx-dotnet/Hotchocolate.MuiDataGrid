namespace Stackworx.Hotchocolate.MuiDataGrid.EnumTypes;

using HotChocolate.Types;

public class MuiDataGridLinkOperatorEnumType : EnumType<MuiDataGridLogicOperator>
{
    protected override void Configure(IEnumTypeDescriptor<MuiDataGridLogicOperator> descriptor)
    {
        descriptor.Name("MuiDataGridLinkOperator");
        descriptor.Value(MuiDataGridLogicOperator.Or).Name("or");
        descriptor.Value(MuiDataGridLogicOperator.And).Name("and");
    }
}