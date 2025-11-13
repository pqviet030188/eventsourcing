using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Demo.EventStore.MartenDB;

public static class MartenStore
{
    private static readonly string STORE_NAME = "property_store";

    public static IServiceCollection AddMartenStore(this IServiceCollection services, string connectionString)
    {
        services.AddMarten(sp =>
        {
            var options = new StoreOptions();
            options.Connection(connectionString);
            options.AutoCreateSchemaObjects = JasperFx.AutoCreate.All;
            options.DatabaseSchemaName = STORE_NAME;

            var mediator = sp.GetRequiredService<IMediator>();
            var projection = new EventProjection(mediator);
            options.Projections.Add(projection, ProjectionLifecycle.Async);

            return options;
        })
            .AddAsyncDaemon(DaemonMode.Solo)
            .UseLightweightSessions();

        // Ensure database schema is up to date on startup
        services.AddHostedService<MartenStartupService>();

        return services;
    }
}

public class MartenStartupService : IHostedService
{
    private readonly IDocumentStore _store;
    public MartenStartupService(IDocumentStore store) => _store = store;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}