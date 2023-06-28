using Domain.Abstractions;

namespace Domain.DomainEvents.Books;

public sealed record BookDescriptionUpdatedDomainEvent(
    Guid Id,
    Guid BookId) : DomainEvent(Id);
