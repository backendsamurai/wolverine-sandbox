namespace WolverineSandbox.Domain.Abstractions;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public Task ConsumeAsync(TEvent @event, CancellationToken ct = default);
}
