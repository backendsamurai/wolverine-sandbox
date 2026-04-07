using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Domain.BoonAggregate;

public sealed record BoonId(Guid Value) : IEntityId;
