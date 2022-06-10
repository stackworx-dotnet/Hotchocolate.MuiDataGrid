namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stackworx.Hotchocolate.MuiDataGrid.EnumTypes;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

public static class IRequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddMuiDataGrid(this IRequestExecutorBuilder builder)
    {
        builder.AddType<MuiValueType>();
        builder.AddType<MuiDataGridLinkOperatorEnumType>();
        builder.AddType<MuiGridSortDirectionEnumType>();
        return builder;
    }
}