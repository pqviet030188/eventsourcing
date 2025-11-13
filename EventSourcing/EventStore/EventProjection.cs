using Marten.Events.Projections;
using Marten;
using MediatR;
using JasperFx.Events;
using Demo.Models.Interfaces;
using Demo.Models;
namespace Demo.EventStore;

public class EventProjection : IProjection
{
    private readonly IMediator _mediator;

    public EventProjection(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ApplyAsync(IDocumentOperations operations, IReadOnlyList<IEvent> events, CancellationToken cancellation)
    {
        var listeningEvents = events.Where(e => e.Data is IDomainEvent);
        
        foreach (var @event in listeningEvents)
        {
            var notificationType = typeof(EventNotification<>).MakeGenericType(@event.GetType());
            var notification = Activator.CreateInstance(notificationType, @event);
            await _mediator.Publish(notification!, cancellation);
        }
    }
}