using Application.Features.BookFeatures.Commands;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.BookFeatures.Validators;

public class BookCreateCommandValidator : AbstractValidator<BookCreateCommand>
{
    public BookCreateCommandValidator(IBookRepository _repository)
    {
        RuleFor(x => x.Title).NotEmpty();
            //.MustAsync(async (title, cancellationToken) => await _repository.IsBookTitleExistAsync(title, cancellationToken))
            //.WithMessage("Book with the same title is already exist.");
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
