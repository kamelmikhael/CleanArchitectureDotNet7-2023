namespace Domain.DomainEvents.Books;

public sealed record BookCreatedDomainEvent(
    Guid Id,
    Guid BookId) : DomainEvent(Id);
