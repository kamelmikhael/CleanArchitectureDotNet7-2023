using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Abstractions.Messaging;
using Application.Common;
using Application.Features.BookFeatures.Dtos;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using Application.Extensions;

namespace Application.Features.BookFeatures.Queries;

public sealed record BookGetAllWithPaginationQuery(BookPagedRequestDto PaginationParams) 
    : IQuery<PagedResponseDto<BookDto>>;

internal sealed class BookGetAllWithPaginationQueryHandler : IQueryHandler<BookGetAllWithPaginationQuery, PagedResponseDto<BookDto>>
{
    private readonly IRepository<Book> _repository;
    private readonly IMapper _mapper;

    public BookGetAllWithPaginationQueryHandler(
        IRepository<Book> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppResult<PagedResponseDto<BookDto>>> Handle(
        BookGetAllWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repository
                .AsNoTracking()
                .AsQueryable();

        query = query.WhereIf(!string.IsNullOrWhiteSpace(request.PaginationParams.Keyword),
            c => c.Title.Contains(request.PaginationParams.Keyword));

        var entitiesPagedResponse = await PagedResponseDto<Book>
            .CreateAsync(query, request.PaginationParams.PageIndex, request.PaginationParams.PageSize);

        var result = PagedResponseDto<BookDto>.Create(
            _mapper.Map<List<BookDto>>(entitiesPagedResponse.Data),
            entitiesPagedResponse.TotalCount,
            entitiesPagedResponse.PageIndex,
            entitiesPagedResponse.PageSize);

        return result;
    }
}
