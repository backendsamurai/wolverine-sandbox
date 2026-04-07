using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Poc.DomainDrivenDesign.Domain.BoonAggregate;
using Poc.DomainDrivenDesign.Domain.Common;
using Poc.DomainDrivenDesign.Persistence.Mongo.Abstractions;
using Poc.DomainDrivenDesign.Persistence.Mongo.Configurations;
using Poc.DomainDrivenDesign.Persistence.Mongo.Repositories;
using Poc.DomainDrivenDesign.Persistence.Mongo.Serializers;

namespace Poc.DomainDrivenDesign.Persistence.Mongo;

public static class MongoRegistration
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services
            .AddOptions<MongoDatabaseConfiguration>()
            .BindConfiguration(MongoDatabaseConfiguration.Section);

        services.AddSingleton<IMongoClient>((serviceProvider) =>
        {
            var mongoDatabaseConfiguration = serviceProvider.GetRequiredService<IOptions<MongoDatabaseConfiguration>>().Value;

            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new EntityIdSerializer<BoonId>());

            return new MongoClient(mongoDatabaseConfiguration.ConnectionString);
        });

        services.AddSingleton((serviceProvider) =>
        {
            var mongoDatabaseConfiguration = serviceProvider.GetRequiredService<IOptions<MongoDatabaseConfiguration>>().Value;
            var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();

            return mongoClient.GetDatabase(mongoDatabaseConfiguration.Database);
        });

        services.AddScoped<IMongoContext, MongoContext>();
        services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
        services.AddScoped<IBoonRepository, BoonRepository>();

        RegisterBsonClassMappings();

        return services;
    }


    private static void RegisterBsonClassMappings()
    {
        BsonClassMap.TryRegisterClassMap<Boon>(cm =>
        {
            cm.AutoMap();
            cm.MapField("_tags").SetElementName(nameof(Boon.Tags));
        });
    }
}
