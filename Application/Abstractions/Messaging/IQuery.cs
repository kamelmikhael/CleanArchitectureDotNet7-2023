using MediatR;

using Domain.Shared;

namespace Application.Abstractions.Messaging;

public interface IQuery : IRequest<AppResult>
{ }

public interface IQuery<TResponse> : IRequest<AppResult<TResponse>>
{ }

