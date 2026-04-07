using Poc.DomainDrivenDesign.Application.Boons.GetByAuthor;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;

namespace Poc.DomainDrivenDesign.Application.Boons;

public static class Mappings
{
    public static BoonByAuthorDto ToBoonByAuthorDto(this Boon boon) 
        => new (boon.Id, boon.Body, boon.Slug, [.. boon.Tags]);
}
