using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using MediatR;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookDeleteCommand(Guid Id) : ICommand;

internal sealed class BookDeleteCommandHandler : ICommandHandler<BookDeleteCommand>
{
    private readonly IBookRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BookDeleteCommandHandler(
        IUnitOfWork unitOfWork,
        IBookRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<AppResult> Handle(BookDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository
                .GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            return AppResult.Failure(DomainErrors.Record.NotFound(nameof(Book), request.Id));
        }

        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(Unit.Value, $"Record with Id = [{entity.Id}] deleted scuccessfly");
    }
}
