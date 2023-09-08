using Domain.Errors;

namespace Domain.Shared;

public class AppResult
{
    protected internal AppResult(bool isSuccess, AppError error, string message = default)
    {
        if (isSuccess && error != AppError.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == AppError.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Errors = new[] { error };
        Message = message;
    }

    protected internal AppResult(bool isSuccess, AppError[] errors, string message = default)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        Message = message;
    }

    public bool IsSuccess { get; set; }

    public bool IsFailure => !IsSuccess;

    public string Message { get; }

    public AppError[] Errors { get; set; } = default;

    public static AppResult Success(string successMessage = default) => new(true, AppError.None, successMessage);

    public static AppResult<TValue> Success<TValue>(TValue value, string successMessage = default)
        => new(value, true, AppError.None, successMessage);

    public static AppResult Failure(AppError error) => new(false, error);

    public static AppResult Failure(AppError[] errors) => new(false, errors);

    public static AppResult<TValue> Failure<TValue>(AppError error)
        => new(default, false, error);

    public static AppResult<TValue> Failure<TValue>(AppError[] errors)
        => new(default, false, errors);

    public static AppResult NotFound(string message)
        => new(false, new AppError("Record.NotFound", message));

    public static AppResult<TValue> Create<TValue>(TValue value, string successMessage = default)
        => new AppResult<TValue>(value, true, AppError.None, successMessage);

    public static AppResult<TValue> Ensure<TValue>(
        TValue value,
        Func<TValue, bool> predicate,
        AppError error) => predicate(value) ? Success(value) : Failure<TValue>(error);

    public static AppResult<TValue> Ensure<TValue>(
        TValue value,
        params (Func<TValue, bool>, AppError)[] functions)
    {
        var results = new List<AppResult<TValue>>();

        foreach ((Func<TValue, bool> predicate, AppError error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }

        return Combine(results.ToArray());
    }

    public static AppResult<TValue> Combine<TValue>(params AppResult<TValue>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            return Failure<TValue>(
                results.SelectMany(r => r.Errors).Distinct().ToArray());
        }

        return Success(results[0].Value);
    }
}

public class AppResult<TValue> : AppResult
{
    private readonly TValue _value;

    public TValue Value => _value;

    protected internal AppResult(TValue value, bool isSuccess, AppError error, string successMessage = default)
        : base(isSuccess, error, successMessage)
    {
        _value = value;
    }

    protected internal AppResult(TValue value, bool isSuccess, AppError[] errors, string successMessage = default)
        : base(isSuccess, errors, successMessage)
    {
        _value = value;
    }

    public static implicit operator AppResult<TValue>(TValue value) => Create(value);
}
