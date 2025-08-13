using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="command">Sale data to be created.</param>
    /// <returns>ID of the newly created sale.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>
    /// Retrieves a sale by its ID.
    /// </summary>
    /// <param name="id">ID of the sale.</param>
    /// <returns>The corresponding sale.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSaleByIdQuery(id));

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Cancels a sale.
    /// </summary>
    /// <param name="id">ID of the sale.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await _mediator.Send(new CancelSaleCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Returns a list of sales with support for pagination and sorting.
    /// </summary>
    /// <param name="query">Filtering, sorting and pagination parameters.</param>
    /// <returns>Paginated list of sales.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<SaleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">ID of the sale to be updated.</param>
    /// <param name="command">Updated sale data.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command)
    {
        if (id != command.Id)
            return BadRequest("The ID in the URL differs from the request body.");

        await _mediator.Send(command);
        return NoContent();
    }
}
