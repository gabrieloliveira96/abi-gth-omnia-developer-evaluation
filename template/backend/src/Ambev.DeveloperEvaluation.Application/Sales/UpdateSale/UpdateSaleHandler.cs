using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IMapper _mapper;



    public UpdateSaleHandler(ISaleRepository saleRepository, ILogger<UpdateSaleHandler> logger, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {

        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errorMessages);
        }

        var sale = await _saleRepository.GetByIdAsync(command.Id);

        if (sale == null)
            throw new Exception("Sale not found");

        sale.Update(
            command.Date,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );

        var newItems = command.Items.Select(i =>
            new SaleItem(i.ProductId, i.ProductName, i.Quantity, i.UnitPrice)
        ).ToList();

        sale.ReplaceItems(newItems);

        var saleUpdate = await _saleRepository.UpdateAsync(sale);

        var result = _mapper.Map<UpdateSaleResult>(saleUpdate);

        var saleCreatedEvent = new SaleUpdatedEvent(sale);

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleUpdatedEvent), sale.Id);

        return result;
    }
}
