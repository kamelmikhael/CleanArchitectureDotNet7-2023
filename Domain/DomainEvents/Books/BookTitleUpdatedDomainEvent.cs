using Domain.Abstractions;

namespace Domain.DomainEvents.Books;

public sealed record BookTitleUpdatedDomainEvent(Guid BookId) : IDomainEvent;
