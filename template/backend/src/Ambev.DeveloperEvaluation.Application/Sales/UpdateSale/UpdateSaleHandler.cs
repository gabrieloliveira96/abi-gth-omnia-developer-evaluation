using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        ILogger<UpdateSaleHandler> logger,
        IMapper mapper)
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

        var previousItems = sale.Items.ToList();

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

        var canceledItems = previousItems
            .Where(old => newItems.All(n => n.ProductId != old.ProductId))
            .ToList();

        sale.ReplaceItems(newItems);

        var updatedSale = await _saleRepository.UpdateAsync(sale);

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);

        var saleUpdatedEvent = new SaleUpdatedEvent(updatedSale);
        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleUpdatedEvent), sale.Id);

        foreach (var canceled in canceledItems)
        {
            var itemCanceledEvent = new ItemCanceledEvent(sale.Id, canceled.Id, "Item removed during update");
            _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}, ItemId: {ItemId}",
                nameof(ItemCanceledEvent), sale.Id, canceled.Id);
        }

        return result;
    }
}
