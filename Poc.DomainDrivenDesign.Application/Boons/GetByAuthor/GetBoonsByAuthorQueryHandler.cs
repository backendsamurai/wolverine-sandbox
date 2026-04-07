using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;
using Poc.DomainDrivenDesign.Domain.BoonAggregate.Specifications;
using System.Reflection;

namespace Poc.DomainDrivenDesign.Application.Boons.GetByAuthor;

public record BoonByAuthorDto(BoonId Id, string Body, string Slug, string[] Tags);

public record GetBoonsByAuthorQuery(Guid AuthorId) : IQuery<IList<BoonByAuthorDto>>;

public class GetBoonsByAuthorQueryHandler : IQueryHandler<GetBoonsByAuthorQuery, IList<BoonByAuthorDto>>
{
    private readonly IBoonRepository _boonRepository;

    public GetBoonsByAuthorQueryHandler(IBoonRepository boonRepository)
    {
        _boonRepository = boonRepository;
    }

    public async Task<IList<BoonByAuthorDto>> HandleAsync(GetBoonsByAuthorQuery query, CancellationToken ct = default)
    {
        var spec = new GetByAuthorIdSpec(query.AuthorId);

        var boons = await _boonRepository.GetAllBoonsAsync(spec, ct);

        return [.. boons.Select(b => b.ToBoonByAuthorDto())];
    }
}
