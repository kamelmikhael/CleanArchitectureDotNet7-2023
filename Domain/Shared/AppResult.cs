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
        Error = error;
        Message = message;
    }

    public bool IsSuccess { get; set; }

    public string Message { get; set; }

    public AppError Error { get; set; } = default;

    public static AppResult Success(string successMessage = default) => new(true, AppError.None, successMessage);

    public static AppResult<TValue> Success<TValue>(TValue value, string successMessage = default)
        => new(value, true, AppError.None, successMessage);

    public static AppResult Failure(AppError error) => new(false, error);

    public static AppResult<TValue> Failure<TValue>(AppError error)
        => new(default, false, error);
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

    public static implicit operator AppResult<TValue>(TValue value) => Create(value);

    public static AppResult<TValue> Create(TValue value, string successMessage = default)
        => new AppResult<TValue>(value, true, AppError.None, successMessage);
}
