using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Features.UserFeatures.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IRepository<User, long> _userRepository;

    public LoginCommandHandler(IJwtProvider jwtProvider,
        IRepository<User, long> userRepository)
    {
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }

    public async Task<AppResult<string>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        //Get User info
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == request.Email);

        if(user is null)
        {
            return AppResult.Failure<string>(DomainErrors.Account.InvalidCredentials);
        }

        // Generate JWT
        return AppResult.Success<string>(_jwtProvider.Generate(user));
    }
}