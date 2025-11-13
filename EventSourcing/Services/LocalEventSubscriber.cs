using Demo.Models;
using Demo.Models.Interfaces;
using MediatR;

namespace Demo.Services;

public class SerialisedEventMessage(string payload) : IRequest<string>
{
    public string Payload { get; } = payload;
}

public class LocalEventSubscriber : IRequestHandler<SerialisedEventMessage, string>
{
    private readonly IMediator _mediator;
    public LocalEventSubscriber(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> Handle(SerialisedEventMessage request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        var @event = EventMessage<IDomainEvent>.Deserialise(payload);
        await _mediator.Publish(@event);
        return "1";
    }
}
