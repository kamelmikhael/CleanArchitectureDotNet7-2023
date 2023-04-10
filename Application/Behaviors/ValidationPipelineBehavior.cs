using Domain.Shared;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : AppResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Validate request
        // If any errors, return validation result
        // Otherwise, return next()

        if(!_validators.Any())
        {
            return await next();
        }

        AppError[] errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new AppError(
                failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if(errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    public static TResult CreateValidationResult<TResult>(AppError[] errors)
        where TResult : AppResult
    {
        if(typeof(TResult) == typeof(AppResult))
        {
            return (AppValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(AppValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(AppValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}
