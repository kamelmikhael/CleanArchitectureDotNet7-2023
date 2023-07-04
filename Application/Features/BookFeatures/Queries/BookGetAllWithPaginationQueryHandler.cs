using AutoMapper;
using Application.Abstractions.Messaging;
using Application.Common;
using Application.Features.BookFeatures.Dtos;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Features.BookFeatures.Queries;

internal sealed class BookGetAllWithPaginationQueryHandler : IQueryHandler<BookGetAllWithPaginationQuery, PagedResponseDto<BookDto>>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BookGetAllWithPaginationQueryHandler(
        IBookRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppResult<PagedResponseDto<BookDto>>> Handle(
        BookGetAllWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        (IEnumerable<Domain.Entities.Book> books, int totalCount) = await _repository
                .GetAllWithPagingAsync(
                    request.PaginationParams.Keyword ?? "",
                    request.PaginationParams.PageIndex, 
                    request.PaginationParams.PageSize,
                    cancellationToken);

        var result = PagedResponseDto<BookDto>.Create(
            _mapper.Map<List<BookDto>>(books),
            totalCount,
            request.PaginationParams.PageIndex,
            request.PaginationParams.PageSize);

        return result;
    }
}
