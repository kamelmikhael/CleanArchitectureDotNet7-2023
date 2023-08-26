using Application.Abstractions;
using Domain.DomainEvents.Books;
using Domain.Repositories;
using MediatR;

namespace Application.Features.BookFeatures.Events;

internal sealed class BookCreatedDomainEventHandler
    : INotificationHandler<BookCreatedDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IBookRepository _bookRepository;

    public BookCreatedDomainEventHandler(
        IEmailService emailService,
        IBookRepository bookRepository)
    {
        _emailService = emailService;
        _bookRepository = bookRepository;
    }

    public async Task Handle(BookCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var book = await _bookRepository
            .GetByIdAsync(notification.BookId, cancellationToken);

        if (book is null) return;

        await _emailService.SendAsync(
            "test@test.com",
            "New Book Created",
            $"New Book with Id = {notification.BookId} Created successfully");
    }
}
