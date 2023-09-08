using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using Domain.ValueObjects;

namespace Application.Features.BookFeatures.Commands;

internal sealed class BookCreateCommandHandler : ICommandHandler<BookCreateCommand, Guid>
{
    private readonly IBookRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IBookRepository repository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppResult<Guid>> Handle(BookCreateCommand request, CancellationToken cancellationToken)
    {
        //if (await _repository.IsBookTitleExistAsync(request.Title))
        //{
        //    return AppResult.Failure<Guid>(DomainErrors.Book.TitleIsAlreadyUsed);
        //}

        //var entity = _mapper.Map<Book>(request);

        AppResult<BookTitle> titleResult = BookTitle.Create(request.Title);

        if (titleResult.IsFailure)
        {
            // Log Error
            return AppResult.Failure<Guid>(titleResult.Errors);
        }

        var isTitleUnique = await _repository.IsBookTitleUniqueAsync(titleResult.Value, cancellationToken);

        var bookCreateResult = Book.Create(
            Guid.NewGuid(),
            titleResult.Value,
            request.Description,
            request.Type,
            request.PublishedOn,
            isTitleUnique);

        if(bookCreateResult.IsFailure)
        {
            return AppResult.Failure<Guid>(bookCreateResult.Errors);
        }

        bookCreateResult.Value.AuthorId = 1;

        _repository.Add(bookCreateResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(
            bookCreateResult.Value.Id,
            $"New record has been added scuccessfly with Id = {bookCreateResult.Value.Id}");
    }
}
