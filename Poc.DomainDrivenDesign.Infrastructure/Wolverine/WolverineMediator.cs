using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Wolverine;

namespace Poc.DomainDrivenDesign.Infrastructure.Wolverine;

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
