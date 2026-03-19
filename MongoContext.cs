using MongoDB.Driver;

namespace WolverineSandbox;

public sealed class MongoContext
{
    public required IMongoDatabase Database { get; set; }

    public required IMongoClient Client { get; set; }

    public IClientSessionHandle? ClientSession { get; set; }
}
