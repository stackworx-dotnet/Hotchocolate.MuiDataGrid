namespace Stackworx.Hotchocolate.MuiDataGrid.EnumTypes;

using HotChocolate.Types;

public class MuiGridSortDirectionEnumType : EnumType<MuiGridSortDirection>
{
    protected override void Configure(IEnumTypeDescriptor<MuiGridSortDirection> descriptor)
    {
        descriptor.Name("MuiGridSortDirection");
        descriptor.Value(MuiGridSortDirection.Asc).Name("asc");
        descriptor.Value(MuiGridSortDirection.Desc).Name("desc");
    }
}