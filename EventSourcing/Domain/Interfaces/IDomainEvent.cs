namespace Demo.Models.Interfaces;

public interface IDomainEvent { 
    Guid EntityId { get; } 
    string Stream {  get; }
}
