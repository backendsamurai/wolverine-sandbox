using Poc.DomainDrivenDesign.Domain.Common;

namespace Poc.DomainDrivenDesign.Domain.BoonAggregate;

public sealed class Boon : AggregateRoot<BoonId>
{
    private List<string> _tags = [];

    public Guid UserId { get; private set; }

    public Guid? ProjectId { get; private set; }

    public string Body { get; private set; } = default!;

    public string Slug { get; private set; } = default!;

    public IReadOnlyList<string> Tags => _tags;

    private Boon(Guid userId, string body, string slug, Guid? projectId = null) : base(new BoonId(Guid.NewGuid()))
    {
        UserId = userId;
        Body = body;
        Slug = slug;
        ProjectId = projectId;
    }

    public static Boon Create(Guid userId, string body, string slug, Guid? projectId, DateTime createdAtUtc, Guid createdBy)
    {
        var boon = new Boon(userId, body, slug, projectId);

        boon.MarkCreated(createdAtUtc, createdBy);
        boon.RaiseEvent(new BoonCreatedDomainEvent(boon.Id));

        return boon;
    }

    public void AddTags(IEnumerable<string> tags)
    {
        _tags.Clear();
        _tags.AddRange(tags);
    }

    public void AddTag(string tag)
    {
        _tags.Add(tag);
    }
}
