using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Book : FullAuditedEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BookType Type { get; set; }
    public DateTime PublishedOn { get; set; }

    public ICollection<BookTranslation> Translations { get; set; } = new List<BookTranslation>();
}

public class BookTranslation
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}
