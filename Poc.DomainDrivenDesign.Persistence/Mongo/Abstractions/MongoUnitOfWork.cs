using Poc.DomainDrivenDesign.Application.Abstractions.Messaging;
using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Persistence.Mongo.Abstractions;

public sealed class MongoUnitOfWork : IUnitOfWork
{
    private readonly IMongoContext _context;
    private readonly IEventBus _bus;
    private readonly List<IAggregateRoot> _tracked = [];

    public MongoUnitOfWork(IMongoContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task BeginAsync(CancellationToken ct = default)
    {
        await _context.StartTransactionAsync(ct);
    }

    public void Track(IAggregateRoot aggregate)
    {
        _tracked.Add(aggregate);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.CommitTransactionAsync(ct);

        foreach (var aggregate in _tracked)
        {
            await _bus.PublishAsync(aggregate.DomainEvents);
            aggregate.ClearEvents();
        }

        _tracked.Clear();
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        await _context.RollbackTransactionAsync(ct);
        _tracked.Clear();
    }
}
