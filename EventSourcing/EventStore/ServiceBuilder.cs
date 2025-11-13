using Demo.EventStore.MartenDB;
using Demo.Models.Options;
using Marten;
using Marten.Events.Projections;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.EventStore;

public static class ServiceBuilder
{
    public static IServiceCollection UseEventStore(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("EventStore");
        services.AddOptions<EventStoreOptions>().Bind(section);

        services.AddSingleton<IProjection>(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();
            return new EventProjection(mediator);
        });

        var eventStoreOptions = section.Get<EventStoreOptions>();
        services.AddMartenStore(eventStoreOptions!.ConnectionString);

        return services;
    }
}