using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Domain.BoonAggregate;

public sealed record BoonCreatedDomainEvent(BoonId Id) : IDomainEvent;
