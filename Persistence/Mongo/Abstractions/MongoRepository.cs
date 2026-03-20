using MongoDB.Driver;
using WolverineSandbox.Domain.Abstractions;
using WolverineSandbox.Persistence.Abstractions;

namespace WolverineSandbox.Persistence.Mongo.Abstractions;

public abstract class MongoRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : IEntityId
{
    private readonly IMongoContext _context;
    private readonly IUnitOfWork _uow;

    protected readonly IMongoCollection<T> collection;

    protected MongoRepository(IMongoContext context, IUnitOfWork uow)
    {
        _context = context;
        collection = _context.GetCollection<T>();
        _uow = uow;
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
        var session = _context.GetCurrentSession()
            ?? throw new InvalidOperationException("Any operations that modify data should be wrapped by session (transaction).");

        await collection.InsertOneAsync(session, entity, cancellationToken: ct);
        _uow.Track(entity);
    }
}
