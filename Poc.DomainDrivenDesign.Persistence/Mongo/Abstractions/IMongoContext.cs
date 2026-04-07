using MongoDB.Driver;
using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Persistence.Mongo.Abstractions;

public interface IMongoContext
{
    public IMongoClient MongoClient { get; }

    public IMongoDatabase MongoDatabase { get; }

    public IMongoCollection<T> GetCollection<T>() where T : IAggregateRoot;

    public IClientSessionHandle? GetCurrentSession();

    public Task StartTransactionAsync(CancellationToken ct = default);

    public Task CommitTransactionAsync(CancellationToken ct = default);

    public Task RollbackTransactionAsync(CancellationToken ct = default);
}
