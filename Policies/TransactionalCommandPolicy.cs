using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.CodeGeneration.Frames;
using JasperFx.Core.Reflection;
using Wolverine.Configuration;
using Wolverine.Runtime.Handlers;
using WolverineSandbox.Attributes;
using WolverineSandbox.Handlers.Abstractions;

namespace WolverineSandbox.Policies;

public sealed class TransactionalCommandPolicy : IHandlerPolicy
{
    public void Apply(IReadOnlyList<HandlerChain> chains, GenerationRules rules, IServiceContainer container)
    {
        foreach (var chain in chains)
        {
            if (!typeof(ICommand).IsAssignableFrom(chain.MessageType))
                continue;

            var attr = chain.Handlers[0].Method.GetAttribute<DisableAutomaticTransactionAttribute>();

            if (attr is not null)
                continue;

            chain.Middleware.Add(new MethodCall(typeof(TransactionalMiddleware), "BeforeAsync"));
            chain.Postprocessors.Add(new MethodCall(typeof(TransactionalMiddleware), "AfterAsync"));
            chain.Postprocessors.Add(new MethodCall(typeof(TransactionalMiddleware), "FinallyAsync"));
        }
    }
}
