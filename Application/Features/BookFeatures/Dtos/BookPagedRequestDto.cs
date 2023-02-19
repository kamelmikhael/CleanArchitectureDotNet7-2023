using Application.Common;

namespace Application.Features.BookFeatures.Dtos;

public class BookPagedRequestDto : PagedRequestDto
{
    /// <summary>
    /// Search Keyword
    /// </summary>
    public string? Keyword { get; set; }
}
