using Microsoft.AspNetCore.Mvc;
using WolverineSandbox.Domain.Boons;
using WolverineSandbox.Handlers;

namespace WolverineSandbox.Controllers;

[ApiController]
[Route("boons")]
public sealed class BoonController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task CreateBoonAsync([FromBody] CreateBoonDTO dto, CancellationToken ct = default) =>
        await _mediator.SendCommandAsync(new CreateBoonCommand(dto), ct);

    [HttpGet("{authorId:guid}")]
    public async Task<IList<Boon>> GetBoonsByAuthorAsync(Guid authorId, CancellationToken ct = default) =>
        await _mediator.SendQueryAsync(new GetBoonsByAuthorQuery(authorId), ct);
}
