using Wolverine;
using WolverineSandbox.Domain.Abstractions;
using WolverineSandbox.Handlers.Abstractions;

namespace WolverineSandbox;


public interface IMediator
{
    public Task<TResponse> SendCommandAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);

    public Task SendCommandAsync(ICommand command, CancellationToken ct = default);

    public Task<TResponse> SendQueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);

}

public interface IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;

    public Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : IDomainEvent;
}


public sealed class Mediator : IMediator
{
    private readonly IMessageBus _messageBus;

    public Mediator(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task<TResponse> SendCommandAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        return await _messageBus.InvokeAsync<TResponse>(command, ct);
    }

    public async Task SendCommandAsync(ICommand command, CancellationToken ct = default)
    {
        await _messageBus.InvokeAsync(command, ct);
    }

    public async Task<TResponse> SendQueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
    {
        return await _messageBus.InvokeAsync<TResponse>(query, ct);
    }
}

public sealed class EventBus : IEventBus
{
    private readonly IMessageBus _messageBus;

    public EventBus(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        await _messageBus.PublishAsync(@event);
    }

    public async Task PublishAsync<TEvent>(IEnumerable<TEvent> events) where TEvent : IDomainEvent
    {
        foreach (var @event in events)
        {
            await _messageBus.PublishAsync(@event);
        }
    }
}
