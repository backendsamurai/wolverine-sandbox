using WolverineSandbox.Contracts;
using WolverineSandbox.Domain.Boons;

namespace WolverineSandbox.Handlers;

public record CreateBoonDTO(string Body);


public sealed class CreateBoonHandler
{
    private readonly IBoonRepository _boonRepository;

    public CreateBoonHandler(IBoonRepository boonRepository)
    {
        _boonRepository = boonRepository;
    }

    public async Task HandleAsync(CreateBoonDTO dto, CancellationToken ct = default)
    {
        var boon = Boon.Create(
            new BoonId(Guid.NewGuid()),
            dto.Body, DateTime.UtcNow, Guid.NewGuid()
        );

        await _boonRepository.InsertBoonAsync(boon, ct);
    }
}
