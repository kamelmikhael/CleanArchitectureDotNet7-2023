using Domain.Common;
using Domain.DomainEvents.Books;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;

//Rich domain model
public sealed class Book : FullAuditedEntity<Guid>
{
    private readonly List<BookTranslation> _translations = new();

    public const int MaxDescriptionLength = 500;
    public const int MaxTypeLength = 50;

    protected Book() { }

    private Book(Guid id, BookTitle title, string description, BookType type, DateOnly publishedOn)
    {
        Id = id;
        Title = title;
        Description = description;
        Type = type;
        PublishedOn = publishedOn;
    }

    public BookTitle Title { get; private set; }

    public string Description { get; private set; }

    public BookType Type { get; private set; }

    public DateOnly PublishedOn { get; private set; }

    public TimeOnly? PublishedTime { get; private set; }

    public int AuthorId { get; set; }

    public IReadOnlyCollection<BookTranslation>? Translations => _translations.AsReadOnly();

    public void SetTitle(BookTitle title)
    {
        // TODO: Validate title here
        Title = title;

        RaiseDomainEvent(new BookTitleUpdatedDomainEvent(
            Guid.NewGuid(),
            Id));
    }

    public void SetDescription(string description)
    {
        // TODO: Validate description here
        Description = description;

        RaiseDomainEvent(new BookDescriptionUpdatedDomainEvent(
            Guid.NewGuid(),
            Id));
    }

    public void AddTranslation(BookTranslation translation)
    {
        // TODO: Validate translation here
        _translations.Add(translation);
    }

    public void AddTranslationRange(List<BookTranslation> translations)
    {
        // TODO: Validate translation here
        _translations.AddRange(translations);
    }

    public static AppResult<Book> Create(
        Guid id,
        BookTitle title,
        string description,
        BookType type,
        DateOnly publishedOn,
        bool isTitleUnique)
    {
        // Validate here
        if (!isTitleUnique)
            return AppResult.Failure<Book>(DomainErrors.Book.TitleAlreadyInUse);

        var book = new Book(
            id,
            title,
            description,
            type,
            publishedOn);

        book.RaiseDomainEvent(new BookCreatedDomainEvent(
            Guid.NewGuid(),
            id));

        return book;
    }
}

public sealed class BookTranslation
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}
