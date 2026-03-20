using WolverineSandbox.Domain.Boons;

namespace WolverineSandbox.Handlers.DomainEventHandlers;

public sealed class BoonCreatedDomainEventHandler(ILogger<BoonCreatedDomainEventHandler> logger)
{
    private readonly ILogger<BoonCreatedDomainEventHandler> _logger = logger;

    public async Task ConsumeAsync(BoonCreatedDomainEvent @event)
    {
        _logger.LogInformation("Boon created: {Id}", @event.Id.Value);
    }
}
