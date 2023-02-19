using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender _sender;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>()!;
}
