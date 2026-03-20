using MongoDB.Driver;
using Wolverine;
using WolverineSandbox.Domain.Abstractions;
using WolverineSandbox.Persistence.Abstractions;

namespace WolverineSandbox.Persistence.Mongo.Abstractions;

public abstract class MongoRepository<T, TId> : IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : IEntityId
{
    private readonly IMongoContext _context;
    private readonly IMessageBus _messageBus;

    protected readonly IMongoCollection<T> collection;

    protected MongoRepository(IMongoContext context, IMessageBus messageBus)
    {
        _context = context;
        collection = _context.GetCollection<T>();
        _messageBus = messageBus;
    }

    public async Task<IList<T>> GetManyAsync(CancellationToken ct = default)
    {
        return await collection
            .Find(_ => true)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<T> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        return await collection
            .Find(e => e.Id.Value == id.Value)
            .FirstOrDefaultAsync(cancellationToken: ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        var session = _context.GetCurrentSession();

        if (session is null)
        {
            await collection.InsertOneAsync(entity, cancellationToken: ct);
            await PublishDomainEvents(entity);

            return;
        }

        await collection.InsertOneAsync(_context.GetCurrentSession(), entity, cancellationToken: ct);
        await PublishDomainEvents(entity);
    }

    private async Task PublishDomainEvents(T entity)
    {
        foreach (var @event in entity.DomainEvents)
        {
            await _messageBus.SendAsync(@event);
        }
    }
}
