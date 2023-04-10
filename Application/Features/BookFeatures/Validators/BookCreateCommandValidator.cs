using Application.Features.BookFeatures.Commands;
using FluentValidation;

namespace Application.Features.BookFeatures.Validators;

public class BookCreateCommandValidator : AbstractValidator<BookCreateCommand>
{
    public BookCreateCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
