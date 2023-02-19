using MediatR;

using Domain.Shared;

namespace Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, AppResult>
    where TCommand : ICommand
{ }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, AppResult<TResponse>>
    where TCommand : ICommand<TResponse>
{ }