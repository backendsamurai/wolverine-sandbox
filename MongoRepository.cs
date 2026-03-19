using Humanizer;
using MongoDB.Driver;

namespace WolverineSandbox;

public abstract class MongoRepository<T>
{
    private readonly MongoContext _context;

    protected readonly IMongoCollection<T> collection;

    protected MongoRepository(MongoContext context)
    {
        _context = context;
        collection = _context.Database.GetCollection<T>(
            typeof(T).Name.Pluralize()
        );
    }

    public T Insert(T document)
    {
        if (_context.ClientSession is not null && _context.ClientSession.IsInTransaction)
        {
            collection.InsertOne(_context.ClientSession, document);
            return document;
        }

        collection.InsertOne(document);

        return document;
    }
}
