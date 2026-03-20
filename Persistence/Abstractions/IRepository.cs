using WolverineSandbox.Domain.Abstractions;

namespace WolverineSandbox.Persistence.Abstractions;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : IEntityId
{
    public Task<IList<T>> GetManyAsync(CancellationToken ct = default);

    public Task<T> GetByIdAsync(TId id, CancellationToken ct = default);

    public Task AddAsync(T entity, CancellationToken ct = default);
}
