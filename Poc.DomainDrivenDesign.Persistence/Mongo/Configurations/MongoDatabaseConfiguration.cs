namespace Poc.DomainDrivenDesign.Persistence.Mongo.Configurations;

public sealed record MongoDatabaseConfiguration
{
    public const string Section = "MongoDatabase";

    public required string ConnectionString { get; init; }

    public required string Database { get; init; }
}
