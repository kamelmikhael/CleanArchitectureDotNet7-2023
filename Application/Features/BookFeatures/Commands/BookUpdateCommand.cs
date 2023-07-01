using Application.Abstractions.Messaging;

namespace Application.Features.BookFeatures.Commands;

public sealed record BookUpdateCommand(Guid Id, string Title, string Description) : ICommand;
