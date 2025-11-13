using Demo.Models.Interfaces;

namespace Demo.EventStore.Events.Property
{
    public record PropertyUpdate(Guid PropertyId, string? Address, decimal? Price) : IDomainEvent
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
                return Models.Constants.EventStore.PROPERTY_STREAM;
            }
        }
    }
}
