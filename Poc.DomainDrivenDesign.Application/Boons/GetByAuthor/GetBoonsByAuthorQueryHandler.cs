using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;
using Poc.DomainDrivenDesign.Domain.BoonAggregate.Specifications;

namespace Poc.DomainDrivenDesign.Application.Boons.GetByAuthor;

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
