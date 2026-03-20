namespace WolverineSandbox.Handlers.Abstractions;

public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    public Task<TResponse> HandleAsync(TQuery query, CancellationToken ct = default);
}

