using Demo.Models.Interfaces;
using JasperFx.Events;
using MediatR;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Demo.Models;

public class EventMessage<T>: INotification where T : IDomainEvent
{
    public EventMessage()
    {

    }
    public EventMessage(Event<T> @event)
    {
        Data = @event.Data;
        Id = @event.Id;
        Version = @event.Version;
        StreamId = @event.StreamId;
        Timestamp = @event.Timestamp;
        TypeName = @event.Data.GetType().FullName!;
    }

    public string TypeName { get; set; }
    public Guid Id { get; set; }
    public long Version { get; set; }
    public Guid StreamId { get; set; }
    public T Data { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public static object Deserialise(string jsonValue)
    {
        dynamic message = JsonNode.Parse(jsonValue)!;
        string typeName = (string)message["TypeName"];
        var eventType = Type.GetType(typeName!);

        var eventMessageType = typeof(EventMessage<>).MakeGenericType(eventType!);

        return JsonSerializer.Deserialize(jsonValue, eventMessageType)!;
    }
}
