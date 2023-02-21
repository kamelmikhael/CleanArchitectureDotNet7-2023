using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookDeleteCommand(Guid Id) : ICommand;

internal sealed class BookDeleteCommandHandler : ICommandHandler<BookDeleteCommand>
{
    private readonly IRepository<Book> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BookDeleteCommandHandler(
        IUnitOfWork repository,
        IRepository<Book> cityRepository)
    {
        _unitOfWork = repository;
        _repository = cityRepository;
    }

    public async Task<AppResult> Handle(BookDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            return AppResult.Failure(DomainErrors.Record.NotFound(nameof(Book), request.Id));
        }

        _repository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(Unit.Value, $"Record with Id = [{entity.Id}] deleted scuccessfly");
    }
}
