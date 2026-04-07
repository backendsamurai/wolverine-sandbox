using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Application.Abstractions.Messaging;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public Task ConsumeAsync(TEvent @event, CancellationToken ct = default);
}
