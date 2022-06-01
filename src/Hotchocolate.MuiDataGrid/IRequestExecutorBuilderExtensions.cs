namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stackworx.Hotchocolate.MuiDataGrid.EnumTypes;

public static class IRequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddMuiDataGrid(this IRequestExecutorBuilder builder)
    {
        builder.AddType<MuiValueScalar>();
        builder.AddType<MuiDataGridLinkOperatorEnumType>();
        builder.AddType<MuiGridSortDirectionEnumType>();
        return builder;
    }
}