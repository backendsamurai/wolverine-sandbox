using JasperFx;
using JasperFx.CodeGeneration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Wolverine;
using WolverineSandbox;
using WolverineSandbox.Contracts;
using WolverineSandbox.Domain.Boons;
using WolverineSandbox.Handlers.Abstractions;
using WolverineSandbox.Persistence.Abstractions;
using WolverineSandbox.Persistence.Mongo.Abstractions;
using WolverineSandbox.Persistence.Mongo.Repositories;
using WolverineSandbox.Persistence.Mongo.Serializers;
using WolverineSandbox.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opt =>
{
    opt.Discovery.DisableConventionalDiscovery();

    opt.Discovery.IncludeAssembly(typeof(Program).Assembly);

    opt.Discovery.CustomizeHandlerDiscovery(q =>
    {
        q.Includes.WithCondition("Must implement handler interface", t =>
            t.GetInterfaces().Any(i => i.IsGenericType && (
                i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
            )));
    });

    opt.Policies.Add<TransactionalCommandPolicy>();

    opt.Services.CritterStackDefaults(x =>
    {
        x.Production.GeneratedCodeMode = TypeLoadMode.Static;
        x.Production.ResourceAutoCreate = AutoCreate.None;

        x.Production.AssertAllPreGeneratedTypesExist = true;

        x.Development.GeneratedCodeMode = TypeLoadMode.Dynamic;
        x.Development.ResourceAutoCreate = AutoCreate.CreateOrUpdate;
    });
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddSingleton<IMongoClient>((_) =>
{
    BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
    BsonSerializer.RegisterSerializer(new EntityIdSerializer<BoonId>());

    return new MongoClient("mongodb://localhost:27017/wolvsandbox");
});

builder.Services.AddSingleton((sp) =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();

    return mongoClient.GetDatabase("wolvsandbox");
});

builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
builder.Services.AddScoped<IBoonRepository, BoonRepository>();
builder.Services.AddScoped<IEventBus, EventBus>();
builder.Services.AddScoped<IMediator, Mediator>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();


app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        logger.LogWarning("GLOBAL ERROR: {msg}", ex.Message);

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
    }
});


return await app.RunJasperFxCommands(args);
