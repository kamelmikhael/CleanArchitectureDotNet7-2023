using Domain.Common;
using Domain.DomainEvents.Books;
using Domain.Enums;

namespace Domain.Entities;

//Rich domain model
public sealed class Book : FullAuditedEntity<Guid>
{
    public const int MaxTitleLength = 50;
    public const int MaxDescriptionLength = 500;
    public const int MaxTypeLength = 50;

    protected Book()
    {
        
    }

    public Book(string title, string description, BookType type, DateOnly publishedOn)
    {
        Title = title;
        Description = description;
        Type = type;
        PublishedOn = publishedOn;
    }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public BookType Type { get; private set; }

    public DateOnly PublishedOn { get; private set; }

    public TimeOnly? PublishedTime { get; private set; }


    private readonly List<BookTranslation> _translations = new();
    public IReadOnlyCollection<BookTranslation>? Translations => _translations.AsReadOnly();

    public void SetTitle(string title)
    {
        // TODO: Validate title here
        Title = title;

        RaiseDomainEvent(new BookTitleUpdatedDomainEvent(Id));
    }

    public void SetDescription(string description)
    {
        // TODO: Validate description here
        Description = description;

        RaiseDomainEvent(new BookDescriptionUpdatedDomainEvent(Id));
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
}

public sealed class BookTranslation
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}
