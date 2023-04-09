namespace Application.Features.BookFeatures.Dtos;

public sealed class BookTranslationDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}