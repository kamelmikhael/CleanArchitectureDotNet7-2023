using Domain.Shared;

namespace Domain.Abstractions;

public interface IAppValidationResult
{
    public static readonly AppError ValidationError = new(
        "Validation.Error",
        "A validation problem occurred.");

    AppError[] Errors { get; }
}