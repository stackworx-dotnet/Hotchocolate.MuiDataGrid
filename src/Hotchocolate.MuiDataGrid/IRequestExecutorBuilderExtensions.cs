namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class IRequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddMuiDataGrid(this IRequestExecutorBuilder builder)
    {
        // TODO
        builder.AddType<MuiValueScalar>();
        return builder;
    }
}