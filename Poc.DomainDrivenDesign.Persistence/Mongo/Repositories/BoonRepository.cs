using MongoDB.Driver;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;
using Poc.DomainDrivenDesign.Domain.Common;
using Poc.DomainDrivenDesign.Persistence.Mongo.Abstractions;

namespace Poc.DomainDrivenDesign.Persistence.Mongo.Repositories;

public sealed class BoonRepository : MongoRepository<Boon, BoonId>, IBoonRepository
{
    public BoonRepository(IMongoContext context, IUnitOfWork unitOfWork)
        : base(context, unitOfWork) { }

    public async Task InsertBoonAsync(Boon boon, CancellationToken ct = default)
    {
        await AddAsync(boon, ct);
    }

    public async Task<Boon> GetBoonByIdAsync(BoonId id, CancellationToken ct = default) =>
        await GetByIdAsync(id, ct);

    public async Task<IList<Boon>> GetAllBoonsAsync(ISpecification<Boon> specification, CancellationToken ct = default)
    {
        return await collection.Find(specification.ToExpression()).ToListAsync(ct);
    }
}
