using Domain.Abstractions;

namespace Domain.Common;

public abstract class Entity : Entity<int>, IEquatable<Entity>
{
    public static bool operator ==(Entity? first, Entity? second)
        => first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity? first, Entity? second)
        => !(first == second);

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        if (other.GetType() != GetType())
            return false;

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != GetType()) return false;

        if (obj is not Entity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
        => Id.GetHashCode() * 41;
}

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>, IAggregateRoot
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>        
    public virtual TPrimaryKey Id { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
