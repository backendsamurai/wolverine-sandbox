using WolverineSandbox.Domain.Abstractions;
using WolverineSandbox.Domain.Boons;

namespace WolverineSandbox.Contracts;

public interface IBoonRepository
{
    public Task<IList<Boon>> GetAllBoonsAsync(ISpecification<Boon> specification, CancellationToken ct = default);

    public Task<Boon> GetBoonByIdAsync(BoonId id, CancellationToken ct = default);

    public Task InsertBoonAsync(Boon boon, CancellationToken ct = default);
}
