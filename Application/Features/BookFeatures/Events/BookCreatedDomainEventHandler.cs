using Application.Abstractions;
using Domain.DomainEvents.Books;
using MediatR;

namespace Application.Features.BookFeatures.Events;

internal sealed class BookCreatedDomainEventHandler
    : INotificationHandler<BookCreatedDomainEvent>
{
    private readonly IEmailService _emailService;

    public BookCreatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(BookCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _emailService.SendAsync("test@test.com", "New Book Created", $"New Book with Id = {notification.BookId} Created successfully");
    }
}
