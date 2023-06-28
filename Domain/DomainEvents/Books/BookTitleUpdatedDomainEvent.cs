using Domain.Abstractions;

namespace Domain.DomainEvents.Books;

public sealed record BookTitleUpdatedDomainEvent(
    Guid Id,
    Guid BookId) : DomainEvent(Id);
