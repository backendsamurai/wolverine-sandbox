namespace WolverineSandbox.Domain.Abstractions;

public abstract class Entity<TId> where TId : IEntityId
{
    public TId Id { get; private set; } = default!;

    public DateTime CreatedAtUtc { get; private set; }

    public Guid? CreatedBy { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public Guid? DeletedBy { get; private set; }

    protected Entity() { }

    protected Entity(TId id) => Id = id;

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
