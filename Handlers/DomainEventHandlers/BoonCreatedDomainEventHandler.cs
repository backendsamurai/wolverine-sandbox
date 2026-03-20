using WolverineSandbox.Domain.Abstractions;
using WolverineSandbox.Domain.Boons;

namespace WolverineSandbox.Handlers.DomainEventHandlers;

public sealed class BoonCreatedDomainEventHandler : IDomainEventHandler<BoonCreatedDomainEvent>
{
    private readonly ILogger<BoonCreatedDomainEventHandler> _logger;

    public BoonCreatedDomainEventHandler(ILogger<BoonCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task ConsumeAsync(BoonCreatedDomainEvent @event, CancellationToken ct = default)
    {
        _logger.LogInformation("Boon created: {Id}", @event.Id.Value);
    }
}
