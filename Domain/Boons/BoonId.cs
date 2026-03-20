using WolverineSandbox.Domain.Abstractions;

namespace WolverineSandbox.Domain.Boons;

public sealed record BoonId(Guid Value) : IEntityId;
