using JasperFx;
using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Infrastructure.Wolverine;
using Poc.DomainDrivenDesign.Persistence.Mongo;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddMongo()
    .AddMessaging(typeof(ICommand).Assembly)
    .AddControllers();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "Poc-DDD");
    });
}

return await app.RunJasperFxCommands(args);
