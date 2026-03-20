namespace WolverineSandbox.Domain.Abstractions;

public interface IAggregateRoot
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    public void RaiseEvent(IDomainEvent @event);

    public void RemoveEvent(IDomainEvent @event);

    public void ClearEvents();
}
