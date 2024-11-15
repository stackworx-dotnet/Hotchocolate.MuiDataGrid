namespace Stackworx.Hotchocolate.MuiDataGrid;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services)
    {
        return services
            .AddDbContextFactory<MuiDataGridDbContext>(builder => builder.ConfigureDbContext());
    }

    public static DbContextOptionsBuilder ConfigureDbContext(this DbContextOptionsBuilder options)
    {
        options.ConfigureWarnings(builder => builder.Ignore(RelationalEventId.PendingModelChangesWarning));
        options.UseSqlite("Filename=test.db");
        return options;
    }
}