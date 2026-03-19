namespace WolverineSandbox;

public sealed class TransactionalMiddleware
{
    private readonly ILogger<TransactionalMiddleware> _logger;

    private readonly MongoContext _mongoContext;

    public TransactionalMiddleware(ILogger<TransactionalMiddleware> logger, MongoContext mongoContext)
    {
        _logger = logger;
        _mongoContext = mongoContext;
    }

    public void Before()
    {
        _logger.LogInformation("WOLVERINE: before execute");
        var session = _mongoContext.Client.StartSession();

        session.StartTransaction();

        _mongoContext.ClientSession = session;
    }

    public void After()
    {
        _logger.LogInformation("WOLVERINE: after execute");

        if (_mongoContext.ClientSession is not null && _mongoContext.ClientSession is { IsInTransaction: true })
        {
            _logger.LogInformation("WOLVERINE: commit transaction;");
            _mongoContext.ClientSession.CommitTransaction();
        }
    }

    public void Finally(Exception? exception)
    {
        _logger.LogInformation("WOLVERINE: finally");

        if (exception is not null && _mongoContext.ClientSession is not null && _mongoContext.ClientSession.IsInTransaction)
        {
            _logger.LogInformation("WOLVERINE: abort transaction;");
            _mongoContext.ClientSession.AbortTransaction();
        }

        _mongoContext.ClientSession?.Dispose();
    }
}
