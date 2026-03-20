using Wolverine;
using WolverineSandbox.Contracts;
using WolverineSandbox.Domain.Boons;
using WolverineSandbox.Persistence.Mongo.Abstractions;

namespace WolverineSandbox.Persistence.Mongo.Repositories;

public sealed class BoonRepository : MongoRepository<Boon, BoonId>, IBoonRepository
{
    public BoonRepository(IMongoContext context, IMessageBus messageBus)
        : base(context, messageBus) { }

    public async Task InsertBoonAsync(Boon boon, CancellationToken ct = default)
    {
        await AddAsync(boon, ct);
    }

    public async Task<IList<Boon>> GetAllBoonsAsync(CancellationToken ct = default) =>
        await GetManyAsync(ct);

    public async Task<Boon> GetBoonByIdAsync(BoonId id, CancellationToken ct = default) =>
        await GetByIdAsync(id, ct);
}
