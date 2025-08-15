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

        var validator = new CancelSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errorMessages);
        }

        var sale = await _saleRepository.GetByIdAsync(request.Id);

        if (sale == null || sale.IsCancelled)
            return false;

        sale.Cancel();

        await _saleRepository.UpdateAsync(sale);

        var saleCreatedEvent = new SaleCanceledEvent(sale);

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleCanceledEvent), sale.Id);

        return true;
    }

}
