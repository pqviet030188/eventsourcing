using Azure.Messaging.ServiceBus;
using Demo.Models;
using Demo.Models.Events;
using Demo.Models.Events.Property;
using Demo.Models.Interfaces;
using Demo.Models.Options;
using JasperFx.Events;
using Microsoft.Extensions.Options;
using System.Text.Json;

/// <summary>
/// Publish event to service bus to construct the view
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventPublisher<T> : IEventPublisher<T> where T : IDomainEvent
{
    private readonly string PROPERTY_EVENT = "Property";
    private readonly Dictionary<string, string> _eventTypeToTopic;
    private readonly Dictionary<string, string> _eventTypeToConnectionString;
    private readonly Dictionary<Type, string> _typeToEventType;
    private readonly Dictionary<string, ServiceBusClient> _eventTypeToClient;

    public EventPublisher(IOptionsMonitor<ServiceBusOptions> serviceBusOptions)
    {
        _eventTypeToTopic = new Dictionary<string, string>();
        _eventTypeToTopic[PROPERTY_EVENT] = serviceBusOptions.CurrentValue.PropertyEventTopic;

        _typeToEventType = new Dictionary<Type, string>()
        {
            { typeof(PropertyCreate), PROPERTY_EVENT },
        };

        _eventTypeToConnectionString = new Dictionary<string, string>();
        _eventTypeToConnectionString[PROPERTY_EVENT] = serviceBusOptions.CurrentValue.PropertyEventConnectionString;

        _eventTypeToClient = new Dictionary<string, ServiceBusClient>();
        _eventTypeToClient[PROPERTY_EVENT] = new ServiceBusClient(_eventTypeToConnectionString[PROPERTY_EVENT]);
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

        if (!_eventTypeToClient.ContainsKey(_typeToEventType[eventType]))
        {
            return;
        }

        var client = _eventTypeToClient[_typeToEventType[eventType]];
        var topic = _eventTypeToTopic[_typeToEventType[eventType]];

        var eventMessageType = typeof(EventMessage<>).MakeGenericType(@event.Data.GetType());
        var eventMessage = Activator.CreateInstance(eventMessageType, @event);

        string messageBody = JsonSerializer.Serialize(eventMessage);
        ServiceBusSender sender = client.CreateSender(topic);
        ServiceBusMessage message = new ServiceBusMessage(messageBody)
        {
            Subject = typeof(T).Name
        };
        await sender.SendMessageAsync(message);
    }
}