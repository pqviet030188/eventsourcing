using Demo.Models.Interfaces;

namespace Demo.Models.Events.Property;

public record PropertyCreate(Guid PropertyId, string Address, decimal Price) : IDomainEvent
{
    public Guid EntityId
    {
        get
        {
            return PropertyId;
        }
    }

    public string Stream 
    {
        get
        {
            return Constants.EventStore.PROPERTY_STREAM;
        }
    }
}
