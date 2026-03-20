namespace Stackworx.Hotchocolate.MudDataGrid;

using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class IRequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddMudDataGridAdapter(this IRequestExecutorBuilder builder)
    {
        builder.Services.AddSingleton<IMudToMuiDataGridAdapter, MudToMuiDataGridAdapter>();
        return builder;
    }
}