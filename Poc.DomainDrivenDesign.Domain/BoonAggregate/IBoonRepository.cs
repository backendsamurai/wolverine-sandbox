using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Domain.BoonAggregate;

public interface IBoonRepository
{
    public Task<IList<Boon>> GetAllBoonsAsync(ISpecification<Boon> specification, CancellationToken ct = default);

    public Task<Boon> GetBoonByIdAsync(BoonId id, CancellationToken ct = default);

    public Task InsertBoonAsync(Boon boon, CancellationToken ct = default);
}
