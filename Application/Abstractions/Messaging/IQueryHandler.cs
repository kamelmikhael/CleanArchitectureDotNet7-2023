using MediatR;

using Domain.Shared;

namespace Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, AppResult>
    where TQuery : IQuery
{ }

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, AppResult<TResponse>>
    where TQuery : IQuery<TResponse>
{ }

