namespace Poc.DomainDrivenDesign.Application.Abstractions.Mediator;

public interface IMediator
{
    public Task<TResponse> ExecuteCommandAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);

    public Task ExecuteCommandAsync(ICommand command, CancellationToken ct = default);

    public Task<TResponse> ExecuteQueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
}
