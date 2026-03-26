using MongoDB.Driver;
using WolverineSandbox.Contracts;
using WolverineSandbox.Domain.Abstractions;
using WolverineSandbox.Domain.Boons;
using WolverineSandbox.Persistence.Abstractions;
using WolverineSandbox.Persistence.Mongo.Abstractions;

namespace WolverineSandbox.Persistence.Mongo.Repositories;

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
