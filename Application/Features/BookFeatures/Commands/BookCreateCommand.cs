using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;
using MediatR;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookCreateCommand(
    string Title, 
    string Description, 
    BookType Type, 
    DateTime PublishedOn) : ICommand<Guid>;

internal sealed class BookCreateCommandHandler : ICommandHandler<BookCreateCommand, Guid>
{
    private readonly IBookRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BookCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IBookRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<AppResult<Guid>> Handle(BookCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            PublishedOn = request.PublishedOn,
        };

        _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AppResult.Success(entity.Id, $"New record has been added scuccessfly with Id = {entity.Id}");
    }
}
