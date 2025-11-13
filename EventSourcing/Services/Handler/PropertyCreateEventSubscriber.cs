using Demo.Models;
using Demo.Models.Events.Property;
using MediatR;

namespace Demo.Services.Handler;

public class PropertyCreateEventSubscriber : INotificationHandler<EventMessage<PropertyCreate>>
{
    
    public async Task Handle(EventMessage<PropertyCreate> notification, CancellationToken cancellationToken)
    {
        // await _eventPublisher.PublishAsync(notification.Event);
        var m = 3;
    }
}
