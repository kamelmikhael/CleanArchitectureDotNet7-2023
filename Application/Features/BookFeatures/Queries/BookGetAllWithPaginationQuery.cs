using Application.Abstractions.Messaging;
using Application.Common;
using Application.Features.BookFeatures.Dtos;
using Domain.Entities;
using Application.Extensions;

namespace Application.Features.BookFeatures.Queries;

public sealed record BookGetAllWithPaginationQuery(BookPagedRequestDto PaginationParams) 
    : IQuery<PagedResponseDto<BookDto>>;
