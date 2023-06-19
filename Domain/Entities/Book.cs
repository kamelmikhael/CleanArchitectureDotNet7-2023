using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Book : FullAuditedEntity<Guid>
{
    protected Book()
    {
        
    }

    public Book(string title, string description, BookType type, DateTime publishedOn)
    {
        Title = title;
        Description = description;
        Type = type;
        PublishedOn = publishedOn;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public BookType Type { get; private set; }
    public DateTime PublishedOn { get; private set; }

    public ICollection<BookTranslation> Translations { get; private set; } = new List<BookTranslation>();

    public void SetTitle(string title)
    {
        // TODO: Validate title here
        Title = title;
    }

    public void SetDescription(string description)
    {
        // TODO: Validate description here
        Description = description;
    }
}

public class BookTranslation
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}
