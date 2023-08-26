using Application.Abstractions.Messaging;

namespace Application.Features.AuthorFeatures.Commands;

public sealed record AuthorCreateCommand(
    string Name,
    DateTime DateOfBirth) : ICommand<int>;
