using Domain.Abstractions;
using Domain.Entities;

namespace Domain.DomainEvents.Books;

public sealed record BookCreatedDomainEvent(Guid BookId) : IDomainEvent;
