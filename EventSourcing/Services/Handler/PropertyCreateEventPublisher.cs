using Demo.Models;
using Demo.Models.Events.Property;
using Demo.Models.Interfaces;
using JasperFx.Events;
using MediatR;

namespace Demo.Services.Handler;

public class PropertyCreateEventPublisher : INotificationHandler<EventNotification<Event<PropertyCreate>>>
{
    private readonly IEventPublisher<PropertyCreate> _eventPublisher;
    
    public PropertyCreateEventPublisher(
       IEventPublisher<PropertyCreate> eventPublisher
    )
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(EventNotification<Event<PropertyCreate>> notification, CancellationToken cancellationToken)
    {
        await _eventPublisher.PublishAsync(notification.Event);
    }
}
