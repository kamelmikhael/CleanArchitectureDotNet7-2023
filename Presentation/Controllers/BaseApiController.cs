using Domain.Abstractions;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender? _sender;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected IActionResult HandleFailure(AppResult result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IAppValidationResult validationResult => BadRequest(
                CreateProblemDetails(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.Errors)),
            _ => BadRequest(
                CreateProblemDetails(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    result.Error))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        AppError error,
        AppError[]? errors = null)
        => new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
