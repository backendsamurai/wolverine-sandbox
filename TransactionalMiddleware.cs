using WolverineSandbox.Persistence.Mongo.Abstractions;

namespace WolverineSandbox;

public sealed class TransactionalMiddleware
{
    private readonly ILogger<TransactionalMiddleware> _logger;

    private readonly IMongoContext _mongoContext;

    public TransactionalMiddleware(ILogger<TransactionalMiddleware> logger, MongoContext mongoContext)
    {
        _logger = logger;
        _mongoContext = mongoContext;
    }

    public async Task BeforeAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("WOLVERINE: before execute");
        await _mongoContext.StartTransactionAsync(ct);
    }

    public async Task AfterAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("WOLVERINE: after execute");
        _logger.LogInformation("WOLVERINE: commit transaction;");
        await _mongoContext.CommitTransactionAsync(ct);
    }

    public async Task FinallyAsync(Exception? exception, CancellationToken ct = default)
    {
        _logger.LogInformation("WOLVERINE: finally");

        if (exception is not null)
        {
            _logger.LogInformation("WOLVERINE: abort transaction;");
            await _mongoContext.RollbackTransactionAsync(ct);
        }
    }
}
