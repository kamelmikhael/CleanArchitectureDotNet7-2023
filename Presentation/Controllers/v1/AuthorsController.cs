using Application.Features.AuthorFeatures.Commands;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.v1;

public class AuthorsController : BaseApiController 
{
    [HttpGet(nameof(GetById) + "/{id:int}")]
    //[ProducesResponseType(typeof(AppResult<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(AppResult<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        AuthorCreateCommand input,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(input, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                nameof(GetById),
                new { id = result.Value },
                result);
    }
}
