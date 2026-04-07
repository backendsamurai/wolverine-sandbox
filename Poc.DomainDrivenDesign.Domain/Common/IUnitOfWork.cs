namespace Poc.DomainDrivenDesign.Domain.Common;

public interface IUnitOfWork
{
    Task BeginAsync(CancellationToken ct = default);

    void Track(IAggregateRoot root);

    Task SaveChangesAsync(CancellationToken ct = default);

    Task RollbackAsync(CancellationToken ct = default);
}
