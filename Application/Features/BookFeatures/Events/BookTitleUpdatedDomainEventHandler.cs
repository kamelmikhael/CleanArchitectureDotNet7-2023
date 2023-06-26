using Application.Abstractions;
using Domain.DomainEvents.Books;
using MediatR;

namespace Application.Features.BookFeatures.Events;

internal sealed class BookTitleUpdatedDomainEventHandler
    : INotificationHandler<BookTitleUpdatedDomainEvent>
{
    private readonly IEmailService _emailService;

    public BookTitleUpdatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(BookTitleUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _emailService.SendAsync("Book Title Updated", "BookTitleUpdated", $"Book Title with Id = {notification.BookId} Updated successfully");
    }
}
