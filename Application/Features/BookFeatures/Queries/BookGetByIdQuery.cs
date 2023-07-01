using Application.Abstractions.Messaging;
using Application.Features.BookFeatures.Dtos;

namespace Application.Features.BookFeatures.Queries;

public sealed record BookGetByIdQuery(Guid Id) : IQuery<BookDto>;
