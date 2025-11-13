using MediatR;

namespace Demo.Models;

public class EventNotification<TEvent> : INotification
{
    public TEvent Event { get; }

    public EventNotification(TEvent @event)
    {
        Event = @event;
    }
}