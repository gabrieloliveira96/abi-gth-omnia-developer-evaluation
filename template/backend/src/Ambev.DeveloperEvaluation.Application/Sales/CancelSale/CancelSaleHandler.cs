using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>

{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CancelSaleHandler> _logger;


    public CancelSaleHandler(ISaleRepository repository, ILogger<CancelSaleHandler> logger)
    {
        _repository = repository;
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

        var sale = await _repository.GetByIdAsync(request.Id);

        if (sale == null || sale.IsCancelled)
            return false;

        sale.Cancel();

        var saleCreatedEvent = new SaleCanceledEvent(sale);

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleCanceledEvent), sale.Id);

        return true;
    }

}
