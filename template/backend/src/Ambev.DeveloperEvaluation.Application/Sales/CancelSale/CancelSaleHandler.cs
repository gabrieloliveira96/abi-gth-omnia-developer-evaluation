using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelSaleHandler> _logger;


    public CancelSaleHandler(ISaleRepository repository, IUnitOfWork unitOfWork, ILogger<CancelSaleHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id);

        if (sale == null || sale.IsCancelled)
            return false;

        sale.Cancel();

        await _unitOfWork.CommitAsync();

        var saleCreatedEvent = new SaleCanceledEvent(sale);

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleCanceledEvent), sale.Id);

        return true;
    }
}
