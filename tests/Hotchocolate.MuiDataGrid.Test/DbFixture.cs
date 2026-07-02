namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.MuiDataGrid.GraphQL;

public class DbFixture : IDisposable
{
    private readonly List<WebApplication> instances = [];

    public DbFixture()
    {
        IServiceCollection services = new ServiceCollection();
        var requestBuilder = this.AddServices(services);

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

        foreach (var instance in this.instances)
        {
            instance.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }

    public Task<MuiDataGridDbContext> CreateDbContextAsync()
    {
        var factory = this.Services.GetRequiredService<IDbContextFactory<MuiDataGridDbContext>>();
        return factory.CreateDbContextAsync();
    }

    public TestServer CreateTestServer()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        builder.Services.AddHttpContextAccessor();
        this.AddServices(builder.Services);

        var app = builder.Build();
        app.UseWebSockets();
        app.MapGraphQL();
        app.StartAsync().GetAwaiter().GetResult();

        this.instances.Add(app);
        return app.GetTestServer();
    }

    private IRequestExecutorBuilder AddServices(IServiceCollection services)
    {
        services.AddDb();

        IRequestExecutorBuilder requestBuilder = services
            .AddLogging(config => { config.SetMinimumLevel(LogLevel.Warning); })
            .AddGraphQL()
            .ModifyRequestOptions(opts =>
            {
                opts.IncludeExceptionDetails = true;
            })
            .AddMuiDataGrid()
            .AddDefaultNodeIdSerializer()
            .RegisterDbContextFactory<MuiDataGridDbContext>()
            .AddQueryType<Query>();

        services.AddRouting()
            .AddGraphQLServer()
            .ModifyCostOptions(o => o.EnforceCostLimits = false);

        return requestBuilder;
    }
}