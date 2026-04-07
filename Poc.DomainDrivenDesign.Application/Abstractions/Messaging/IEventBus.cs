using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Application.Abstractions.Messaging;

public interface IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

    public Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent;
}
