using Demo.EventStore.Events.Property;
using Demo.Models;
using Demo.Models.Interfaces;
using JasperFx.Events;
using MediatR;

namespace Demo.Services.Handler;

public class PropertyUpdateEventPublisher : INotificationHandler<EventNotification<Event<PropertyUpdate>>>
{
    private readonly IEventPublisher<PropertyUpdate> _eventPublisher;

    public PropertyUpdateEventPublisher(
       IEventPublisher<PropertyUpdate> eventPublisher
    )
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(EventNotification<Event<PropertyUpdate>> notification, CancellationToken cancellationToken)
    {
        await _eventPublisher.PublishAsync(notification.Event);
    }
}
