using System.Linq.Expressions;
using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Domain.BoonAggregate.Specifications;

public sealed class GetBoonByIdSpec(BoonId id) : Specification<Boon>
{
    public override Expression<Func<Boon, bool>> ToExpression()
    {
        return boon => boon.Id == id;
    }
}

public sealed class GetByAuthorIdSpec(Guid authorId) : Specification<Boon>
{
    public override Expression<Func<Boon, bool>> ToExpression()
    {
        return boon => boon.CreatedBy == authorId;
    }
}
