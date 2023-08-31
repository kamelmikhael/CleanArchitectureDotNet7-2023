using Domain.Shared;

namespace Domain.Extensions;

public static class AppResultExtensions
{
    public static AppResult<T> Ensure<T>(
        this AppResult<T> result,
        Func<T, bool> predicate,
        AppError error)
    {
        if(result.IsFailure) return result;

        return predicate(result.Value)
            ? result
            : AppResult.Failure<T>(error);
    }

    public static AppResult<TOut> Map<TIn, TOut>(
        this AppResult<TIn> result,
        Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess
            ? AppResult.Success(mappingFunc(result.Value))
            : AppResult.Failure<TOut>(result.Error);
    }
}
