using Application.Common;
using Application.Features.BookFeatures.Commands;
using Application.Features.BookFeatures.Dtos;
using Application.Features.BookFeatures.Queries;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Persistence.v1;

public class BooksController : BaseApiController
{
    /// <summary>
    /// Get List With Pagination
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(nameof(GetListWithPagination))]
    public async Task<AppResult<PagedResponseDto<BookDto>>> GetListWithPagination(
        [FromQuery] BookPagedRequestDto input, 
        CancellationToken cancellationToken)
        => await Sender.Send(
            new BookGetAllWithPaginationQuery(input), cancellationToken
        );

    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(nameof(GetById) + "/{id:guid}")]
    public async Task<AppResult<BookDto>> GetById(
        Guid id, 
        CancellationToken cancellationToken)
        => await Sender.Send(
            new BookGetByIdQuery(id), cancellationToken
        );

    /// <summary>
    /// Create new Record
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(nameof(Create))]
    public async Task<AppResult> Create(
        BookCreateCommand input
        , CancellationToken cancellationToken)
        => await Sender.Send(
            input, cancellationToken
        );

    /// <summary>
    /// Update record
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(nameof(Update))]
    public async Task<AppResult> Update(
        BookUpdateCommand input
        , CancellationToken cancellationToken)
        => await Sender.Send(
            input, cancellationToken
        );

    /// <summary>
    /// Delete record
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(nameof(Delete) + "/{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new BookDeleteCommand(id), cancellationToken
        );

        return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
    }
}
