using WolverineSandbox.Domain.Abstractions;

namespace WolverineSandbox.Domain.Boons;

public sealed class Boon : AggregateRoot<BoonId>
{
    public string Body { get; private set; } = null!;

    private Boon() { }

    private Boon(BoonId id, string body) : base(id)
    {
        Body = body;
    }

    public static Boon Create(BoonId id, string body, DateTime createdAtUtc, Guid createdBy)
    {
        var boon = new Boon(id, body);

        boon.MarkCreated(createdAtUtc, createdBy);
        boon.RaiseEvent(new BoonCreatedDomainEvent(boon.Id));

        return boon;
    }
}
