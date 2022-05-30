namespace Stackworx.Hotchocolate.MuiDataGrid;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services)
    {
        return services
            .AddDbContextFactory<MuiDataGridDbContext>(options => options.UseSqlite("Filename=test.db"));
    }
}