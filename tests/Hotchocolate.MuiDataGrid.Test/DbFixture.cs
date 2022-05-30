namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileMart.Switch.Test.MuiDataGrid.GraphQL;
using Stackworx.Hotchocolate.MuiDataGrid;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public class DbFixture : IDisposable
{
    public DbFixture()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddDb();

        IRequestExecutorBuilder requestBuilder = services
            .AddGraphQL()
            .AddMuiDataGrid()
            .ModifyOptions(o => o.EnableOneOf = true)
            .RegisterDbContext<MuiDataGridDbContext>()
            .AddQueryType<Query>();

        this.Services = services.BuildServiceProvider();
        this.RequestExecutor = requestBuilder.BuildRequestExecutorAsync().GetAwaiter().GetResult();

        var factory = services.BuildServiceProvider().GetRequiredService<IDbContextFactory<MuiDataGridDbContext>>();
        var dbContext = factory.CreateDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    public ServiceProvider Services { get; }

    public IRequestExecutor RequestExecutor { get; }

    public void Dispose()
    {
        this.Services.Dispose();
    }

    public Task<MuiDataGridDbContext> CreateDbContextAsync()
    {
        var factory = this.Services.GetRequiredService<IDbContextFactory<MuiDataGridDbContext>>();
        return factory.CreateDbContextAsync();
    }
}