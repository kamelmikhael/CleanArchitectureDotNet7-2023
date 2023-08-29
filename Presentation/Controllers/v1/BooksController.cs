using Application.Common;
using Application.Features.BookFeatures.Commands;
using Application.Features.BookFeatures.Dtos;
using Application.Features.BookFeatures.Queries;
using Domain.Shared;
using Infrastructure.Authentications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.v1;

//[HasPermission(Permission.BookAll)]
public class BooksController : BaseApiController
{
    /// <summary>
    /// Get List With Pagination
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(nameof(GetListWithPagination))]
    [ProducesResponseType(typeof(AppResult<PagedResponseDto<BookDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListWithPagination(
        [FromQuery] BookPagedRequestDto input, 
        CancellationToken cancellationToken)
        => Ok(await Sender.Send(
            new BookGetAllWithPaginationQuery(input), cancellationToken
        ));

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(nameof(GetById) + "/{id:guid}")]
    [ProducesResponseType(typeof(AppResult<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var response = await Sender.Send(
            new BookGetByIdQuery(id), cancellationToken
        );

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Create new Record
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(AppResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        BookCreateCommand input, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(input, cancellationToken);

        return result.IsFailure 
            ? HandleFailure(result) 
            : CreatedAtAction(
                nameof(GetById),
                new { id = result.Value },
                result);
    }

    /// <summary>
    /// Update record
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(nameof(Update))]
    [ProducesResponseType(typeof(AppResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        BookUpdateCommand input
        , CancellationToken cancellationToken)
    {
        //TODO: Handle validations errors

        var response = await Sender.Send(input, cancellationToken);

        return response.IsSuccess ? Ok(response) : NotFound(response.Error);
    }

    /// <summary>
    /// Delete record
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(nameof(Delete) + "/{id:guid}")]
    public async Task<AppResult> Delete(
        Guid id,
        CancellationToken cancellationToken) 
        => await Sender.Send(
            new BookDeleteCommand(id), cancellationToken
        );
}
