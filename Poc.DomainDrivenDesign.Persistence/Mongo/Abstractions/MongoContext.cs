using Humanizer;
using MongoDB.Driver;
using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Persistence.Mongo.Abstractions;

public sealed class MongoContext : IMongoContext
{
    private IClientSessionHandle? _clientSession;

    public IMongoClient MongoClient { get; private set; }

    public IMongoDatabase MongoDatabase { get; private set; }

    private bool IsTransactionActive => _clientSession is not null && _clientSession.IsInTransaction;

    public MongoContext(IMongoClient mongoClient, IMongoDatabase mongoDatabase)
    {
        MongoClient = mongoClient;
        MongoDatabase = mongoDatabase;
    }

    public IMongoCollection<T> GetCollection<T>() where T : IAggregateRoot
    {
        return MongoDatabase.GetCollection<T>(typeof(T).Name.Pluralize());
    }

    public IClientSessionHandle? GetCurrentSession() => IsTransactionActive ? _clientSession : null;

    public async Task StartTransactionAsync(CancellationToken ct = default)
    {
        if (_clientSession != null)
            return;

        _clientSession = await MongoClient.StartSessionAsync(cancellationToken: ct);
        _clientSession.StartTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_clientSession == null)
            return;

        await _clientSession.CommitTransactionAsync(ct);
        _clientSession.Dispose();
        _clientSession = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_clientSession == null)
            return;

        await _clientSession.AbortTransactionAsync(ct);
        _clientSession.Dispose();
        _clientSession = null;
    }
}
