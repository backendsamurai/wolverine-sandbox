using System.Reflection;
using JasperFx;
using JasperFx.CodeGeneration;
using Microsoft.Extensions.DependencyInjection;
using Poc.DomainDrivenDesign.Application.Abstractions.Mediator;
using Poc.DomainDrivenDesign.Application.Abstractions.Messaging;
using Poc.DomainDrivenDesign.Infrastructure.Wolverine.Policies;
using Wolverine;

namespace Poc.DomainDrivenDesign.Infrastructure.Wolverine;

public static class WolverineRegistration
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, Assembly assembly)
    {
        services.AddWolverine(opt =>
        {
            opt.Discovery.DisableConventionalDiscovery();

            opt.Discovery.IncludeAssembly(assembly);

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

        services.AddScoped<IEventBus, EventBus>();
        services.AddScoped<IMediator, Mediator>();

        return services;
    }
}
