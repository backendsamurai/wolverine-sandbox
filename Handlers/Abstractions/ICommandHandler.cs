namespace WolverineSandbox.Handlers.Abstractions;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    public Task<TResponse> HandleAsync(TCommand command, CancellationToken ct = default);
}
