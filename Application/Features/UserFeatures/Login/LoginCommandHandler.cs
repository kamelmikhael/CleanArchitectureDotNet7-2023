using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.UserFeatures.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    public async Task<AppResult<string>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        //Get User info
        var user = new User() { Id = 1, Name = "Kamel Danial", Email = request.Email };

        if(user is null)
        {
            return AppResult.Failure<string>(DomainErrors.Account.InvalidCredentials);
        }

        // Generate JWT
        return AppResult.Success<string>(_jwtProvider.Generate(user));
    }
}