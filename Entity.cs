namespace WolverineSandbox;

public interface IDomainEvent { }

public interface IHasTimestamps
{
    public DateTime CreatedAtUtc { get; }

    public DateTime? UpdatedAtUtc { get; }
}

public interface IAuditable
{
    public Guid? CreatedBy { get; }

    public Guid? UpdatedBy { get; }

    public Guid? DeletedBy { get; }
}

public abstract class Entity
{
    public Guid Id { get; private set; }

    public DateTime CreatedAtUtc;

    public Guid? CreatedBy;

    public DateTime? UpdatedAtUtc;

    public Guid? UpdatedBy;

    public DateTime? DeletedAtUtc;

    public Guid? DeletedBy;

    protected virtual void MarkCreated(DateTime createdAtUtc, Guid? createBy = null)
    {
        CreatedAtUtc = createdAtUtc;
        CreatedBy = createBy;
    }

    protected virtual void MarkUpdated(DateTime updatedAtUtc, Guid? updatedBy = null)
    {
        UpdatedAtUtc = updatedAtUtc;
        UpdatedBy = updatedBy;
    }

    protected virtual void MarkDeleted(DateTime deletedAtUtc, Guid? deletedBy = null)
    {
        DeletedAtUtc = deletedAtUtc;
        DeletedBy = deletedBy;
    }
}

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void RaiseEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
}


public sealed class Project : AggregateRoot
{

    public void ChangeStatus(DateTime updatedAtUtc)
    {
        MarkUpdated(updatedAtUtc);
    }
}
