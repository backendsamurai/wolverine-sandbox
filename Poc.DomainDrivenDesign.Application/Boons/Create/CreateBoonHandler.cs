using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;

namespace Poc.DomainDrivenDesign.Application.Boons.Create;

public record CreateBoonDTO
{
    public Guid? ProjectId { get; set; }

    public required string Body { get; set; }

    public List<string> Tags { get; set; } = [];
}

public record CreateBoonCommand(CreateBoonDTO Dto) : ICommand;

public sealed class CreateBoonHandler : ICommandHandler<CreateBoonCommand>
{
    private readonly Guid _userId = Guid.Parse("0D3EB442-79FA-4A59-A6FB-23A0FF42B8DF");
    private readonly IBoonRepository _boonRepository;

    public CreateBoonHandler(IBoonRepository boonRepository)
    {
        _boonRepository = boonRepository;
    }

    public async Task HandleAsync(CreateBoonCommand command, CancellationToken ct = default)
    {
        var boon = Boon.Create(
            _userId, command.Dto.Body,
            "test-slug", command.Dto.ProjectId,
            DateTime.UtcNow, _userId
        );

        boon.AddTags(command.Dto.Tags);

        await _boonRepository.InsertBoonAsync(boon, ct);
    }
}
