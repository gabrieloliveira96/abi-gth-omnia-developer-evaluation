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
        try
        {
            var validator = new CancelSaleItemCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogInformation("validationResult: {errorMessages}", errorMessages);
                throw new ValidationException(errorMessages);
            }

            var sale = await _saleRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                throw new InvalidOperationException("Item not found");

            item.Cancel();

            item.AddDomainEvent(new ItemCanceledEvent(sale.Id, item.Id, "Item canceled"));

            await _saleRepository.UpdateAsync(sale);

            return true;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            throw; 
        }
    }

}
