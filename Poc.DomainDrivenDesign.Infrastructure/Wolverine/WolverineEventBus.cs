using Poc.DomainDrivenDesign.Application.Abstractions.Messaging;
using Poc.DomainDrivenDesign.Domain.Common;
using Wolverine;

namespace Poc.DomainDrivenDesign.Infrastructure.Wolverine;

public sealed class EventBus : IEventBus
{
    private readonly IMessageBus _messageBus;

    public EventBus(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        await _messageBus.PublishAsync(@event);
    }

    public async Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent
    {
        foreach (var @event in events)
        {
            await _messageBus.PublishAsync(@event);
        }
    }
}
