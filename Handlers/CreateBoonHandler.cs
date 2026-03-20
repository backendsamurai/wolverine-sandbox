using WolverineSandbox.Attributes;
using WolverineSandbox.Contracts;
using WolverineSandbox.Domain.Boons;
using WolverineSandbox.Handlers.Abstractions;
using WolverineSandbox.Persistence.Abstractions;

namespace WolverineSandbox.Handlers;

public record CreateBoonDTO(string Body);

public record CreateBoonCommand(CreateBoonDTO Dto) : ICommand;


public sealed class CreateBoonHandler : ICommandHandler<CreateBoonCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IBoonRepository _boonRepository;

    public CreateBoonHandler(IBoonRepository boonRepository, IUnitOfWork uow)
    {
        _boonRepository = boonRepository;
        _uow = uow;
    }

    [DisableAutomaticTransaction]
    public async Task HandleAsync(CreateBoonCommand command, CancellationToken ct = default)
    {
        await _uow.BeginAsync(ct);

        try
        {
            var boon = Boon.Create(
                new BoonId(Guid.NewGuid()),
                command.Dto.Body, DateTime.UtcNow, Guid.NewGuid()
            );

            await _boonRepository.InsertBoonAsync(boon, ct);
            await _uow.SaveChangesAsync(ct);
        }
        catch
        {
            await _uow.RollbackAsync(ct);
        }
    }

    // With automatic transaction handled by Wolverine
    //
    // public async Task HandleAsync(CreateBoonCommand command, CancellationToken ct = default)
    // {
    //     var boon = Boon.Create(
    //         new BoonId(Guid.NewGuid()),
    //         command.Dto.Body, DateTime.UtcNow, Guid.NewGuid()
    //     );

    //     await _boonRepository.InsertBoonAsync(boon, ct);
    // }
}
