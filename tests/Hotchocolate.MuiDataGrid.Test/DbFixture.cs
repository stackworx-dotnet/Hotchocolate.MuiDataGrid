namespace Stackworx.Hotchocolate.MuiDataGrid;

using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.MuiDataGrid.GraphQL;

public class DbFixture : IDisposable
{
    private readonly List<TestServer> instances = [];

    public DbFixture()
    {
        IServiceCollection services = new ServiceCollection();
        var (_, requestBuilder) = this.AddServices(services);

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
            instance.Dispose();
        }
    }

    public Task<MuiDataGridDbContext> CreateDbContextAsync()
    {
        var factory = this.Services.GetRequiredService<IDbContextFactory<MuiDataGridDbContext>>();
        return factory.CreateDbContextAsync();
    }

    public TestServer CreateTestServer()
    {
        IWebHostBuilder builder = new WebHostBuilder()
            .Configure(
                app =>
                {
                    app
                        .UseWebSockets()
                        .UseRouting();

                    app.UseEndpoints(
                        endpoints =>
                        {
                            endpoints.MapGraphQL();
                        });
                })
            .ConfigureServices(
                services =>
                {
                    services.AddHttpContextAccessor();
                    this.AddServices(services);
                });

        var server = new TestServer(builder);
        this.instances.Add(server);
        return server;
    }

    private (IServiceCollection services, IRequestExecutorBuilder builder) AddServices(IServiceCollection services)
    {
        services.AddDb();

        IRequestExecutorBuilder requestBuilder = services
            .AddLogging(config => { config.SetMinimumLevel(LogLevel.Warning); })
            .AddGraphQL()
            .AddMuiDataGrid()
            .AddDefaultNodeIdSerializer()
            .ModifyOptions(o =>
            {
                o.EnableOneOf = true;
            })
            .RegisterDbContextFactory<MuiDataGridDbContext>()
            .AddQueryType<Query>();

        services.AddRouting()
            // .AddHttpResultSerializer(HttpResultSerialization.JsonArray)
            .AddGraphQLServer()
            .ModifyCostOptions(o => o.EnforceCostLimits = false);

        return (services, requestBuilder);
    }
}