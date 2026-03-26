using System.Linq.Expressions;
using WolverineSandbox.Domain.Abstractions;

namespace WolverineSandbox.Domain.Boons.Specifications;

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