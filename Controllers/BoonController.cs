using Microsoft.AspNetCore.Mvc;
using Wolverine;
using WolverineSandbox.Handlers;

namespace WolverineSandbox.Controllers;

[ApiController]
[Route("boons")]
public sealed class BoonController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public BoonController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpPost]
    public async Task CreateBoonAsync([FromBody] CreateBoonDTO dto, CancellationToken ct = default) =>
        await _messageBus.InvokeAsync(dto, ct);
}
