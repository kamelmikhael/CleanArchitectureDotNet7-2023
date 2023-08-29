using Application.Features.BookFeatures.Dtos;
using Application.Features.UserFeatures.Login;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.v1;

public class AccountController : BaseApiController 
{
    [HttpPost(nameof(Login))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest input,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(input.Email);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure 
            ? HandleFailure(result)
            : Ok(result.Value);
    }
}
