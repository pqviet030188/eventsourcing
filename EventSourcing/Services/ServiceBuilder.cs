using Demo.EventStore.Events.Property;
using Demo.Models.Events.Property;
using Demo.Models.Interfaces;
using Demo.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Services;

public static class ServiceBuilder
{
    public static IServiceCollection UseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ServiceBusOptions>().Bind(configuration.GetSection("ServiceBus"));
        //services.AddSingleton<IEventPublisher<PropertyCreate>, EventPublisher<PropertyCreate>>();
        //services.AddSingleton<IEventPublisher<PropertyUpdate>, EventPublisher<PropertyUpdate>>();

        services.AddSingleton<IEventPublisher<PropertyCreate>, LocalEventPublisher<PropertyCreate>>();
        services.AddSingleton<IEventPublisher<PropertyUpdate>, LocalEventPublisher<PropertyUpdate>>();
        return services;
    }
}