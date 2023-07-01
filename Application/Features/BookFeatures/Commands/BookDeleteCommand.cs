using Application.Abstractions.Messaging;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookDeleteCommand(Guid Id) : ICommand;
