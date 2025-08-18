using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>

{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;


    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }
    public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = new CancelSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogInformation("validationResult: {errorMessages}", errorMessages);
                throw new ValidationException(errorMessages);
            }

            var sale = await _saleRepository.GetByIdAsync(request.Id);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            if (sale.IsCancelled)
                throw new InvalidOperationException("Sale already is canceled");

            sale.Cancel();

            var existingItems = sale.Items.ToList();

            foreach (var item in existingItems)
            {
                if (!item.IsCancelled)
                    item.Cancel();

                item.AddDomainEvent(new ItemCanceledEvent(sale.Id, item.Id, "Item removed during update"));
            }

            sale.AddDomainEvent(new SaleCanceledEvent(sale));

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
