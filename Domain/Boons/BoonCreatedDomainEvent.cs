using WolverineSandbox.Domain.Abstractions;

namespace WolverineSandbox.Domain.Boons;

public sealed record BoonCreatedDomainEvent(BoonId Id) : IDomainEvent;
