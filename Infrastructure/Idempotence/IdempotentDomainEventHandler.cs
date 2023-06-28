using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Infrastructure.Contexts;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Idempotence;

public sealed class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly ApplicationDbContext _dbContext;

    public IdempotentDomainEventHandler(
        INotificationHandler<TDomainEvent> decorated,
        ApplicationDbContext dbContext)
    {
        _decorated = decorated;
        _dbContext = dbContext;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        string consumer = _decorated.GetType().Name;

        bool isConsumerExist = await _dbContext
            .Set<OutboxMessageConsumer>()
            .AnyAsync(
                outboxMessageConsumer =>
                    outboxMessageConsumer.Id == notification.Id &&
                    outboxMessageConsumer.Name == consumer, 
                cancellationToken);

        if (isConsumerExist)
        {
            return;
        }

        await _decorated.Handle(notification, cancellationToken);

        _dbContext
            .Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer,
            });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
