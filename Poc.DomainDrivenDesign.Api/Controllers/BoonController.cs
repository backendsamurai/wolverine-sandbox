using Microsoft.AspNetCore.Mvc;
using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Application.Boons.Create;
using Poc.DomainDrivenDesign.Application.Boons.GetByAuthor;

namespace Poc.DomainDrivenDesign.Api.Controllers;

[ApiController]
[Route("boons")]
public sealed class BoonController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task CreateBoonAsync([FromBody] CreateBoonDTO dto, CancellationToken ct = default)
    {
        await mediator.ExecuteCommandAsync(new CreateBoonCommand(dto), ct);
    }

    [HttpGet("{authorId:guid}")]
    public async Task<IList<BoonByAuthorDto>> GetBoonsByAuthorAsync(Guid authorId, CancellationToken ct = default)
    {
        return await mediator.ExecuteQueryAsync(new GetBoonsByAuthorQuery(authorId), ct);
    }
}
