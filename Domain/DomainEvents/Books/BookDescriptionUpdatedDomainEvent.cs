using Domain.Abstractions;

namespace Domain.DomainEvents.Books;

public sealed record BookDescriptionUpdatedDomainEvent(Guid BookId) : IDomainEvent;
