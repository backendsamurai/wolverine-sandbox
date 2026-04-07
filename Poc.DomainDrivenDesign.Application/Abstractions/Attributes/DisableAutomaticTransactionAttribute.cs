namespace Poc.DomainDrivenDesign.Application.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class DisableAutomaticTransactionAttribute : Attribute
{
}
