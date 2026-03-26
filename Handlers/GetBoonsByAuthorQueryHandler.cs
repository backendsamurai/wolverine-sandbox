using WolverineSandbox.Contracts;
using WolverineSandbox.Domain.Boons;
using WolverineSandbox.Domain.Boons.Specifications;
using WolverineSandbox.Handlers.Abstractions;

namespace WolverineSandbox.Handlers;

public record GetBoonsByAuthorQuery(Guid AuthorId) : IQuery<IList<Boon>>;

public class GetBoonsByAuthorQueryHandler : IQueryHandler<GetBoonsByAuthorQuery, IList<Boon>>
{
    private readonly IBoonRepository _boonRepository;

    public GetBoonsByAuthorQueryHandler(IBoonRepository boonRepository)
    {
        _boonRepository = boonRepository;
    }

    public async Task<IList<Boon>> HandleAsync(GetBoonsByAuthorQuery query, CancellationToken ct = default)
    {
        var spec = new GetByAuthorIdSpec(query.AuthorId);

        return await _boonRepository.GetAllBoonsAsync(spec, ct);
    }
}
