using Demo.EventStore.Events.Property;
using Demo.Models;
using Demo.Models.Events.Property;
using MediatR;

namespace Demo.Services.Handler;

public class PropertyUpdateEventSubscriber : INotificationHandler<EventMessage<PropertyUpdate>>
{

    public async Task Handle(EventMessage<PropertyUpdate> notification, CancellationToken cancellationToken)
    {
        // await _eventPublisher.PublishAsync(notification.Event);
        var m = 3;
    }
}
