namespace Stackworx.Hotchocolate.MudDataGrid;

using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stackworx.Hotchocolate.MuiDataGrid.Types;

public static class IRequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddMudDataGridAdapter(this IRequestExecutorBuilder builder)
    {
        builder.AddType(new MuiValueType("MudValue"));
        builder.AddType<MudDataGridFilterInput>();
        builder.AddType<MudDataGridFilterDefinitionInput>();
        builder.AddType<MudDataGridSortDefinitionInput>();
        return builder;
    }
}