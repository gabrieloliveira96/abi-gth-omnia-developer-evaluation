using MediatR;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDetailsResponse?>
{
    private readonly ISaleRepository _repository;

    public GetSaleByIdQueryHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<SaleDetailsResponse?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id);

        if (sale is null)
            return null;

        return new SaleDetailsResponse
        {
            Id = sale.Id,
            Date = sale.Date,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            IsCancelled = sale.IsCancelled,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new SaleItemDetailsResponse
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
