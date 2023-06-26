using Application.Abstractions;
using Domain.DomainEvents.Books;
using MediatR;

namespace Application.Features.BookFeatures.Events;

internal sealed class BookDescriptionUpdatedDomainEventHandler
    : INotificationHandler<BookTitleUpdatedDomainEvent>
{
    private readonly IEmailService _emailService;

    public BookDescriptionUpdatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(BookTitleUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _emailService.SendAsync("Book Description Updated", "BookDescriptionUpdated", $"Book Description with Id = {notification.BookId} Updated successfully");
    }
}
