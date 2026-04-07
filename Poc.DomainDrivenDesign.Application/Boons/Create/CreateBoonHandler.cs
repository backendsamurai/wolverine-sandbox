using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;

namespace Poc.DomainDrivenDesign.Application.Boons.Create;

public record CreateBoonDTO(string Body);

public record CreateBoonCommand(CreateBoonDTO Dto) : ICommand;

public sealed class CreateBoonHandler : ICommandHandler<CreateBoonCommand>
{
    private readonly IBoonRepository _boonRepository;

    public CreateBoonHandler(IBoonRepository boonRepository)
    {
        _boonRepository = boonRepository;
    }

    public async Task HandleAsync(CreateBoonCommand command, CancellationToken ct = default)
    {
        var boon = Boon.Create(
            new BoonId(Guid.NewGuid()),
            command.Dto.Body, DateTime.UtcNow, Guid.NewGuid()
        );

        await _boonRepository.InsertBoonAsync(boon, ct);
    }
}
