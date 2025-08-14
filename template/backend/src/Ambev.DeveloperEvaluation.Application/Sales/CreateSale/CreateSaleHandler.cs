using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Guid>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSaleHandler> _logger;


    public CreateSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;

    }

    public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Sale;
        var sale = new Sale(dto.Date, dto.CustomerId, dto.CustomerName, dto.BranchId, dto.BranchName);

        foreach (var item in dto.Items)
        {
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        await _saleRepository.AddAsync(sale);
        await _unitOfWork.CommitAsync();

        var saleCreatedEvent = new SaleCreatedEvent(sale);

        _logger.LogInformation("Event generated: {EventName} for SaleId: {SaleId}", nameof(SaleCreatedEvent), sale.Id);


        // Aqui seria possível logar/publishar evento SaleCreated

        return sale.Id;
    }
}
