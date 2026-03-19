using JasperFx;
using JasperFx.CodeGeneration;
using MongoDB.Driver;
using Wolverine;
using WolverineSandbox;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opt =>
{
    opt.Policies.AddMiddleware<TranslationalMiddleware>();

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

builder.Services.AddSingleton<IMongoClient>((_) =>
{
    return new MongoClient("mongodb://localhost:27017/wolvsandbox");
});

builder.Services.AddSingleton((sp) =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();

    return mongoClient.GetDatabase("wolvsandbox");
});

builder.Services.AddSingleton((sp) =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var mongoDatabase = sp.GetRequiredService<IMongoDatabase>();

    return new MongoContext
    {
        Client = mongoClient,
        Database = mongoDatabase,
    };
});


var app = builder.Build();

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


app.MapGet("/test", async (IMessageBus mb) =>
{
    var todo = await mb.InvokeAsync<TodoItem>(new CreateTodo("Title"));

    return todo;
});


return await app.RunJasperFxCommands(args);
