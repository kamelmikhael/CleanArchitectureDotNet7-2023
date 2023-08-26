namespace Domain.Abstractions;

/// <summary>
/// Generic IBaseEntity interface for most used primary key type (System.Int32)
/// </summary>
public interface IBaseEntity : IEntity<int>
{ }

/// <summary>
/// Generic IBaseEntity interface
/// </summary>
/// <typeparam name="TPrimaryKey"></typeparam>
public interface IEntity<TPrimaryKey>
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>
    TPrimaryKey Id { get; set; }
}

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}

