using Domain.Abstractions;

namespace Domain.Common;

public abstract class BaseEntity : BaseEntity<int>, IEquatable<BaseEntity>
{
    public static bool operator ==(BaseEntity? first, BaseEntity? second)
        => first is not null && second is not null && first.Equals(second);

    public static bool operator !=(BaseEntity? first, BaseEntity? second)
        => !(first == second);

    public bool Equals(BaseEntity? other)
    {
        if (other is null)
            return false;

        if (other.GetType() != GetType())
            return false;

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        if (obj is not BaseEntity entity)
            return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}

public abstract class BaseEntity<TPrimaryKey> : IBaseEntity<TPrimaryKey>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>        
    public virtual TPrimaryKey Id { get; set; }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
