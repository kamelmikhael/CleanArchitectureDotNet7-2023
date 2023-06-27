using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookUpdateCommand(Guid Id, string Title, string Description) : ICommand;

public sealed class BookUpdateCommandHandler : ICommandHandler<BookUpdateCommand>
{
    private readonly IBookRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BookUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        IBookRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<AppResult> Handle(BookUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository
                .GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            return AppResult.Failure(DomainErrors.Record.NotFound(nameof(Book), request.Id));
        }

        AppResult<BookTitle> titleResult = BookTitle.Create(request.Title);

        if(titleResult.IsFailure)
        {
            // Log Error
            return titleResult;
        }

        entity.SetTitle(titleResult.Value);
        entity.SetDescription(request.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(Unit.Value, $"Record with Id = [{entity.Id}] updated scuccessfly");
    }
}
