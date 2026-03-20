using JasperFx;
using JasperFx.CodeGeneration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Wolverine;
using WolverineSandbox;
using WolverineSandbox.Contracts;
using WolverineSandbox.Persistence.Mongo.Abstractions;
using WolverineSandbox.Persistence.Mongo.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opt =>
{
    opt.Policies.AddMiddleware<TransactionalMiddleware>();

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

    return new MongoClient("mongodb://localhost:27017/wolvsandbox");
});

builder.Services.AddSingleton((sp) =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();

    return mongoClient.GetDatabase("wolvsandbox");
});

builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IBoonRepository, BoonRepository>();

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
