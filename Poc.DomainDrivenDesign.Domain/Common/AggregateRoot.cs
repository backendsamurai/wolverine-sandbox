namespace Poc.DomainDrivenDesign.Domain.Common;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : IEntityId
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot() { }

    protected AggregateRoot(TId id) : base(id) { }

    public void RaiseEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearEvents() => _domainEvents.Clear();

    public void RemoveEvent(IDomainEvent @event) => _domainEvents.Remove(@event);
}
