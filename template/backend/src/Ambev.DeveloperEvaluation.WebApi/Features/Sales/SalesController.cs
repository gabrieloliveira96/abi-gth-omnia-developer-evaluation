using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<SalesController> _logger;

    public SalesController(IMediator mediator, IMapper mapper, ILogger<SalesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="command">Sale data to be created.</param>
    /// <returns>ID of the newly created sale.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var createdId = response.Id;
        var locationUri = Url.Action(nameof(GetSaleById), "Sales", new { id = createdId }, Request.Scheme);

        return Created(
            routeName: nameof(GetSaleById),
            routeValues: new { id = response.Id },
            data: _mapper.Map<CreateSaleResponse>(response)
        );
    }

    /// <summary>
    /// Retrieves a sale by its ID.
    /// </summary>
    /// <param name="id">ID of the sale.</param>
    /// <returns>The corresponding sale.</returns>
    [HttpGet("{id}", Name = "GetSaleById")]
    [ProducesResponseType(typeof(GetSaleByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSaleByIdRequest { Id = id };
        var validator = new GetSaleByIdValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSaleByIdCommand>(request.Id);
        var response = await _mediator.Send(command, cancellationToken);
        if (response == null)
        {
            return NotFound(message: "Sale with ID {command.Id} not found");
        }
        return Ok(data: _mapper.Map<GetSaleByIdResponse>(response));
    }
    /// <summary>
    /// Cancels a sale.
    /// </summary>
    /// <param name="id">ID of the sale.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CanceledSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new CancelSaleRequest { Id = id };
        var validator = new CancelSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CancelSaleCommand>(request.Id);
        await _mediator.Send(command, cancellationToken);
    
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Sale is canceled successfully"
        });

    }
    /// <summary>
    /// Returns a paginated list of sales with filters and sorting.
    /// </summary>
    [HttpGet("paginated")]
    [ProducesResponseType(typeof(PaginatedResponse<GetSalesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesRequest request, CancellationToken cancellationToken)
    {
        var validator = new GetSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSalesCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new PaginatedResponse<GetSalesResponse>
        {
            CurrentPage = request.PageNumber,
            TotalPages = (int)Math.Ceiling((double)result.TotalCount / request.PageSize),
            TotalCount = result.TotalCount,
            Data = _mapper.Map<IEnumerable<GetSalesResponse>>(result.Data),
            Success = true,
            Message = "Sales retrieved successfully"
        });
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">ID of the sale to be updated.</param>
    /// <param name="request">Updated sale data.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest(message:"The ID in the URL differs from the request body.");
            
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(data: _mapper.Map<UpdateSaleResponse>(result));

    }
    /// <summary>
    /// Cancels a specific sale item.
    /// </summary>
    /// <param name="saleId">ID of the sale.</param>
    /// <param name="itemId">ID of the item to cancel.</param>
    [HttpDelete("{saleId}/items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSaleItem([FromRoute] Guid saleId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var request = new CancelSaleItemRequest
        {
            SaleId = saleId,
            ItemId = itemId
        };

        var validator = new CancelSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CancelSaleItemCommand>(request);

        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Sale item canceled successfully"
        });
    }

}
