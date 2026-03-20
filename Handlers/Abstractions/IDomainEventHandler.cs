using WolverineSandbox.Domain.Abstractions;

namespace WolverineSandbox.Handlers.Abstractions;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public Task ConsumeAsync(TEvent @event, CancellationToken ct = default);
}
