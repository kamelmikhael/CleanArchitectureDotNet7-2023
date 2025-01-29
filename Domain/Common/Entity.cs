using Domain.Abstractions;

namespace Domain.Common;

public abstract class Entity : Entity<int>
{ }

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>, IAggregateRoot, IEquatable<Entity<TPrimaryKey>>
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>        
    public virtual TPrimaryKey Id { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public static bool operator ==(Entity<TPrimaryKey> first, Entity<TPrimaryKey> second) => first.Equals(second);

    public static bool operator !=(Entity<TPrimaryKey> first, Entity<TPrimaryKey> second) => !first.Equals(second);

    public bool Equals(Entity<TPrimaryKey>? other)
    {
        if (other is null || other.GetType() != GetType()) return false;

        return EqualityComparer<TPrimaryKey>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TPrimaryKey>);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}
