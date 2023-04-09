using Domain.Abstractions;
using Domain.Entities;

namespace Domain.DomainEvents;

public sealed record BookCreatedDomainEvent(Book Book) : IDomainEvent;
