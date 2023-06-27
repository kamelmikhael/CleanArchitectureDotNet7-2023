using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using Domain.ValueObjects;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookCreateCommand(
    string Title, 
    string Description, 
    BookType Type,
    DateOnly PublishedOn,
    List<BookTranslationDto> Translations) : ICommand<Guid>;

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
            return AppResult.Failure<Guid>(titleResult.Error);
        }

        var entity = new Book(
            Guid.NewGuid(),
            titleResult.Value,
            request.Description,
            request.Type,
            request.PublishedOn);

        _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(entity.Id, $"New record has been added scuccessfly with Id = {entity.Id}");
    }
}
