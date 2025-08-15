using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, bool>

{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleItemHandler> _logger;


    public CancelSaleItemHandler(ISaleRepository saleRepository, ILogger<CancelSaleItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }
    public async Task<bool> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {

        var validator = new CancelSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errorMessages);
        }

        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale == null)
            return false;

        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item == null)
            return false;

        item.Cancel();

        await _saleRepository.UpdateAsync(sale);

        var saleCreatedEvent = new ItemCanceledEvent(request.SaleId,request.ItemId,"Item canceled");

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleCanceledEvent), sale.Id);

        return true;
    }

}
