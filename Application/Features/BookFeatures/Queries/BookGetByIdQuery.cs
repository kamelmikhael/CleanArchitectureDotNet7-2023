using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BookFeatures.Queries;

public sealed record BookGetByIdQuery(Guid Id) : IQuery<BookDto>;

internal sealed class BookGetByIdQueryHandler : IQueryHandler<BookGetByIdQuery, BookDto>
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BookGetByIdQueryHandler(
        IBookRepository repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppResult<BookDto>> Handle(BookGetByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository
            .GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            return AppResult.Failure<BookDto>(DomainErrors.Record.NotFound(nameof(Book), request.Id));
        }

        var dto = _mapper.Map<BookDto>(entity);

        return dto;
    }
}
