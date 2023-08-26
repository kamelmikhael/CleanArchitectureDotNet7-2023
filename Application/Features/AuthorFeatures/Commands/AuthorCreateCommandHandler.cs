using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Domain.UnitOfWorks;

namespace Application.Features.AuthorFeatures.Commands;

internal sealed class AuthorCreateCommandHandler : ICommandHandler<AuthorCreateCommand, int>
{
    private readonly IRepository<Author, int> _authorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthorCreateCommandHandler(
        IRepository<Author,
        int> authorRepository,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppResult<int>> Handle(AuthorCreateCommand request, CancellationToken cancellationToken)
    {
        bool isExist = _authorRepository
            .AsNoTracking()
            .Any(x => x.Name == request.Name);

        if (isExist)
        {
            return AppResult.Failure<int>(DomainErrors.Author.NameAlreadyExist);
        }

        var authorResult = Author.Create(
            request.Name,
            request.DateOfBirth);

        if (authorResult.IsFailure) return AppResult.Failure<int>(authorResult.Error);

        _authorRepository.Add(authorResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return authorResult.Value.Id;
    }
}
