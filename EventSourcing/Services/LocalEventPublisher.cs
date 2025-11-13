using Demo.Models;
using Demo.Models.Events.Property;
using Demo.Models.Interfaces;
using Demo.Models.Options;
using JasperFx.Events;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Demo.Services;

/// <summary>
/// Publish event to service bus to construct the view
/// </summary>
/// <typeparam name="T"></typeparam>
public class LocalEventPublisher<T> : IEventPublisher<T> where T : IDomainEvent
{
    private readonly string PROPERTY_EVENT = "Property";
    private readonly Dictionary<string, string> _eventTypeToTopic;
    private readonly Dictionary<string, string> _eventTypeToConnectionString;
    private readonly Dictionary<Type, string> _typeToEventType;
    private readonly IMediator _mediator;
    public LocalEventPublisher(IOptionsMonitor<ServiceBusOptions> serviceBusOptions, IMediator mediator)
    {
        _eventTypeToTopic = new Dictionary<string, string>();
        _eventTypeToTopic[PROPERTY_EVENT] = serviceBusOptions.CurrentValue.PropertyEventTopic;

        _typeToEventType = new Dictionary<Type, string>()
        {
            { typeof(PropertyCreate), PROPERTY_EVENT },
        };

        _eventTypeToConnectionString = new Dictionary<string, string>();
        _eventTypeToConnectionString[PROPERTY_EVENT] = serviceBusOptions.CurrentValue.PropertyEventConnectionString;

        _mediator = mediator;
    }

    public async Task PublishAsync(Event<T> @event)
    {
        var eventType = @event.Data.GetType();

        if (!_typeToEventType.ContainsKey(eventType))
        {
            return;
        }

        if (!_eventTypeToTopic.ContainsKey(_typeToEventType[eventType]))
        {
            return;
        }

        var eventMessageType = typeof(EventMessage<>).MakeGenericType(@event.Data.GetType());
        var eventMessage = Activator.CreateInstance(eventMessageType, @event);

        string messageBody = JsonSerializer.Serialize(eventMessage);
        await _mediator.Send(new SerialisedEventMessage(messageBody));
    }
}