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
    private readonly IRepository<Book> _repository;
    private readonly IMapper _mapper;

    public BookGetByIdQueryHandler(IRepository<Book> cityRepository
        , IMapper mapper)
    {
        _repository = cityRepository;
        _mapper = mapper;
    }

    public async Task<AppResult<BookDto>> Handle(BookGetByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository
            .AsNoTracking()
            .AsQueryable()
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            return AppResult.Failure<BookDto>(DomainErrors.Record.NotFound(nameof(Book), request.Id));
        }

        return entity;
    }
}
