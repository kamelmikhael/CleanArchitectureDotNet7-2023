using MediatR;

using Domain.Shared;

namespace Application.Abstractions.Messaging;

public interface ICommand : IRequest<AppResult>
{ }

public interface ICommand<TResponse> : IRequest<AppResult<TResponse>>
{ }
