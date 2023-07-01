using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;
using Domain.Enums;
using Domain.Errors;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookCreateCommand(
    string Title, 
    string Description, 
    BookType Type,
    DateOnly PublishedOn,
    List<BookTranslationDto> Translations) : ICommand<Guid>;
