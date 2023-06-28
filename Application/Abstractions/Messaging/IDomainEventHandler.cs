using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{ }
