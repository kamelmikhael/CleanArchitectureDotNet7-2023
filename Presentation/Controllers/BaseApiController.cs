using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender? _sender;

    //public BaseApiController(ISender sender)
    //{
    //    _sender = sender;
    //}

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>()!;
}
