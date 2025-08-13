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

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSaleByIdQuery(id));

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await _mediator.Send(new CancelSaleCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
    [HttpGet]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID da URL difere do corpo da requisição.");

        await _mediator.Send(command);
        return NoContent();
    }
}
