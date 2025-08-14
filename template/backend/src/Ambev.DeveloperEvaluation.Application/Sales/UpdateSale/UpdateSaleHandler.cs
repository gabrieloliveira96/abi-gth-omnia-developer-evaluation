using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, Unit>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSaleHandler> _logger;


    public UpdateSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id);
        if (sale == null)
            throw new Exception("Sale not found");

        sale.Update(
            request.Date,
            request.CustomerId,
            request.CustomerName,
            request.BranchId,
            request.BranchName
        );

        var newItems = request.Items.Select(i =>
            new SaleItem(i.ProductId, i.ProductName, i.Quantity, i.UnitPrice)
        ).ToList();

        sale.ReplaceItems(newItems);

        await _unitOfWork.CommitAsync();

        var saleCreatedEvent = new SaleUpdatedEvent(sale);

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleUpdatedEvent), sale.Id);

        return Unit.Value;
    }
}
