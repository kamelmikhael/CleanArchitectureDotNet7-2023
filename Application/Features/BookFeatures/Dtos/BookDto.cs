using Application.Common;
using Domain.Enums;

namespace Application.Features.BookFeatures.Dtos;

public class BookDto : BaseEntityDto<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; }
}