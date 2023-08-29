using Application.Abstractions.Messaging;

namespace Application.Features.UserFeatures.Login;

public record LoginCommand(string Email) : ICommand<string>;
