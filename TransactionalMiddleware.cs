using WolverineSandbox.Persistence.Abstractions;

namespace WolverineSandbox;

public sealed class TransactionalMiddleware
{
    private readonly ILogger<TransactionalMiddleware> _logger;
    private readonly IUnitOfWork _uow;

    public TransactionalMiddleware(IUnitOfWork uow, ILogger<TransactionalMiddleware> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task BeforeAsync()
    {
        _logger.LogInformation("TRANSACTIONAL MIDDLEWARE : BEFORE EXECUTION");
        await _uow.BeginAsync();
    }

    public async Task AfterAsync()
    {
        _logger.LogInformation("TRANSACTIONAL MIDDLEWARE : AFTER EXECUTION");
        await _uow.SaveChangesAsync();
    }

    public async Task FinallyAsync(Exception? exception)
    {
        _logger.LogInformation("TRANSACTIONAL MIDDLEWARE : FINALLY");

        if (exception is not null)
        {
            await _uow.RollbackAsync();
        }
    }
}
