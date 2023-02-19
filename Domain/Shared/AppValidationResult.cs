using Domain.Abstractions;
namespace Domain.Shared;

public sealed class AppValidationResult : AppResult, IAppValidationResult
{
    public AppError[] Errors { get; }

    private AppValidationResult(AppError[] errors)
        : base(false, IAppValidationResult.ValidationError) =>
        Errors = errors;

    public static AppValidationResult WithErrors(AppError[] errors) => new(errors);
}

public sealed class AppValidationResult<TValue> : AppResult<TValue>, IAppValidationResult
{
    public AppError[] Errors { get; }

    private AppValidationResult(AppError[] errors)
        : base(default, false, IAppValidationResult.ValidationError) =>
        Errors = errors;

    public static AppValidationResult<TValue> WithErrors(AppError[] errors) => new(errors);
}
