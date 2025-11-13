using JasperFx.Events;

namespace Demo.Models.Interfaces;

public interface IEventPublisher<T> where T : IDomainEvent
{
    Task PublishAsync(Event<T> @event);
}
