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

public sealed record BookUpdateCommand(Guid Id, string Title, string Description) : ICommand;

public sealed class BookUpdateCommandHandler : ICommandHandler<BookUpdateCommand>
{
    private readonly IRepository<Book> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BookUpdateCommandHandler(
        IUnitOfWork repository, 
        IRepository<Book> cityRepository)
    {
        _unitOfWork = repository;
        _repository = cityRepository;
    }

    public async Task<AppResult> Handle(BookUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            return AppResult.Failure(DomainErrors.Record.NotFound);
        }

        entity.Title = request.Title;
        entity.Description = request.Description;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(Unit.Value, $"Record with Id = [{entity.Id}] updated scuccessfly");
    }
}
